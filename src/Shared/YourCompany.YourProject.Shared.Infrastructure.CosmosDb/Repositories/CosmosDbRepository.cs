using System.Linq.Expressions;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Helpers;

namespace YourCompany.YourProject.Shared.Infrastructure.CosmosDb.Repositories;

/// <summary>
/// Generic Cosmos DB repository implementation with automatic partition key extraction.
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public class CosmosDbRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntityBase
{
    protected readonly Container Container;
    protected readonly string ContainerName;
    protected readonly string PartitionKeyPath;

    public CosmosDbRepository(CosmosClient cosmosClient, string databaseName)
    {
        ContainerName = CollectionNameHelper.GetCollectionName<TEntity>();
        PartitionKeyPath = PartitionKeyHelper.GetPartitionKeyPath<TEntity>();
        Container = cosmosClient.GetContainer(databaseName, ContainerName);
    }

    /// <summary>
    /// Extracts the partition key value from the entity.
    /// </summary>
    private PartitionKey GetPartitionKey(TEntity entity)
    {
        var partitionKeyValue = PartitionKeyHelper.GetPartitionKeyValue(entity);
        return new PartitionKey(partitionKeyValue);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var partitionKey = GetPartitionKey(entity);
        var response = await Container.CreateItemAsync(entity, partitionKey);
        return response.Resource;
    }

    public async Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            // For Cosmos DB, we need the partition key, so we query by id instead
            var iterator = Container
                .GetItemLinqQueryable<TEntity>()
                .Where(e => e.Id == id)
                .ToFeedIterator();

            var response = await iterator.ReadNextAsync(cancellationToken);
            var entity = response.FirstOrDefault();

            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            return entity;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found.", ex);
        }
    }

    public async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var iterator = Container.GetItemLinqQueryable<TEntity>().Where(predicate).ToFeedIterator();

        var response = await iterator.ReadNextAsync(cancellationToken);
        var entity = response.FirstOrDefault();

        if (entity == null)
        {
            throw new KeyNotFoundException("Entity not found.");
        }

        return entity;
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = Container.GetItemLinqQueryable<TEntity>().ToFeedIterator();
        var results = new List<TEntity>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);
            results.AddRange(response);
        }

        return results;
    }

    public async Task<List<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var query = Container.GetItemLinqQueryable<TEntity>().Where(predicate).ToFeedIterator();

        var results = new List<TEntity>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync(cancellationToken);
            results.AddRange(response);
        }

        return results;
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var iterator = Container
                .GetItemLinqQueryable<TEntity>()
                .Where(e => e.Id == id)
                .Take(1)
                .ToFeedIterator();

            var response = await iterator.ReadNextAsync(cancellationToken);
            return response.Any();
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var iterator = Container
            .GetItemLinqQueryable<TEntity>()
            .Where(predicate)
            .Take(1)
            .ToFeedIterator();

        var response = await iterator.ReadNextAsync(cancellationToken);
        return response.Any();
    }

    public async Task<long> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var iterator = Container.GetItemLinqQueryable<TEntity>().Where(predicate).ToFeedIterator();

        long count = 0;
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            count += response.Count;
        }

        return count;
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await GetAsync(id);
        var partitionKey = GetPartitionKey(entity);
        await Container.DeleteItemAsync<TEntity>(id, partitionKey);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await QueryAsync(predicate);

        foreach (var entity in entities)
        {
            var partitionKey = GetPartitionKey(entity);
            await Container.DeleteItemAsync<TEntity>(entity.Id, partitionKey);
        }
    }

    public async Task<TEntity> ReplaceAsync(TEntity entity)
    {
        var partitionKey = GetPartitionKey(entity);
        var response = await Container.ReplaceItemAsync(entity, entity.Id, partitionKey);
        return response.Resource;
    }

    public async Task<TEntity> UpdateAsync(string id, Action<TEntity> updateAction)
    {
        var entity = await GetAsync(id);
        updateAction(entity);
        return await ReplaceAsync(entity);
    }

    public async Task<TEntity> UpdateAsync(string id, Func<TEntity, Task> updateAction)
    {
        var entity = await GetAsync(id);
        await updateAction(entity);
        return await ReplaceAsync(entity);
    }

    public Task<TEntity> UpsertAsync(TEntity entity)
    {
        var partitionKey = GetPartitionKey(entity);
        return Container.UpsertItemAsync(entity, partitionKey).ContinueWith(t => t.Result.Resource);
    }

    public async Task<List<TEntity>> ExecuteRawQueryAsync(string query, params object[] parameters)
    {
        // CosmosDB SQL queries don't support traditional parameterized queries like SQL Server
        // Parameters must be embedded in the QueryDefinition
        var queryDefinition = new QueryDefinition(query);

        // Add parameters using @paramN convention
        for (int i = 0; i < parameters.Length; i++)
        {
            queryDefinition = queryDefinition.WithParameter($"@param{i}", parameters[i]);
        }

        var iterator = Container.GetItemQueryIterator<TEntity>(queryDefinition);

        var results = new List<TEntity>();
        // Execute query and consume all results
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }
}
