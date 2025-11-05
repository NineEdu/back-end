using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Helpers;

namespace YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

/// <summary>
/// Generic MongoDB repository implementation with automatic shard key extraction.
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public class MongoDbRepository<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntityBase
{
    protected readonly IMongoCollection<TEntity> Collection;
    protected readonly string CollectionName;

    public MongoDbRepository(IMongoDatabase database)
    {
        CollectionName = CollectionNameHelper.GetCollectionName<TEntity>();
        Collection = database.GetCollection<TEntity>(CollectionName);
    }

    public MongoDbRepository(IMongoCollection<TEntity> collection)
    {
        Collection = collection;
        CollectionName = CollectionNameHelper.GetCollectionName<TEntity>();
    }

    /// <summary>
    /// Builds a filter that includes shard key if available.
    /// This improves query performance in sharded collections.
    /// </summary>
    protected virtual FilterDefinition<TEntity> BuildFilterWithShardKey(
        string id,
        TEntity? entity = null
    )
    {
        var builder = Builders<TEntity>.Filter;
        var filter = builder.Eq(e => e.Id, id);

        // If entity is provided and has shard key, include it in the filter
        if (entity != null && ShardKeyHelper.HasShardKey<TEntity>())
        {
            try
            {
                var shardKey = ShardKeyHelper.GetShardKeyValue(entity);
                foreach (var kvp in shardKey)
                {
                    var property = typeof(TEntity).GetProperty(kvp.Key);
                    if (property != null)
                    {
                        filter &= builder.Eq(kvp.Key, kvp.Value);
                    }
                }
            }
            catch
            {
                // If shard key extraction fails, use id only
            }
        }

        return filter;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await Collection.InsertOneAsync(entity);
        return entity;
    }

    public virtual async Task<TEntity> GetAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        var entity = await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        return entity;
    }

    public virtual async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await Collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException("Entity not found.");
        }

        return entity;
    }

    public virtual async Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await Collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await Collection.Find(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        var count = await Collection.CountDocumentsAsync(
            filter,
            cancellationToken: cancellationToken
        );
        return count > 0;
    }

    public virtual async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var count = await Collection.CountDocumentsAsync(
            predicate,
            cancellationToken: cancellationToken
        );
        return count > 0;
    }

    public virtual async Task<long> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await Collection.CountDocumentsAsync(
            predicate,
            cancellationToken: cancellationToken
        );
    }

    public virtual async Task DeleteAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        var result = await Collection.DeleteOneAsync(filter);

        if (result.DeletedCount == 0)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }
    }

    public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await Collection.DeleteManyAsync(predicate);
    }

    public virtual async Task<TEntity> ReplaceAsync(TEntity entity)
    {
        var filter = BuildFilterWithShardKey(entity.Id, entity);
        var result = await Collection.ReplaceOneAsync(filter, entity);

        if (result.MatchedCount == 0)
        {
            throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");
        }

        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(string id, Action<TEntity> updateAction)
    {
        var entity = await GetAsync(id);
        updateAction(entity);
        return await ReplaceAsync(entity);
    }

    public virtual async Task<TEntity> UpdateAsync(string id, Func<TEntity, Task> updateAction)
    {
        var entity = await GetAsync(id);
        await updateAction(entity);
        return await ReplaceAsync(entity);
    }

    public async Task<TEntity> UpsertAsync(TEntity entity)
    {
        var filter = BuildFilterWithShardKey(entity.Id, entity);
        var options = new ReplaceOptions { IsUpsert = true };
        await Collection.ReplaceOneAsync(filter, entity, options);
        return entity;
    }

    public async Task<List<TEntity>> ExecuteRawQueryAsync(string query, params object[] parameters)
    {
        var bsonQuery = new BsonDocument
        {
            { "query", query },
            { "parameters", new BsonArray(parameters.Select(p => BsonValue.Create(p))) },
        };

        var command = new BsonDocument { { "find", CollectionName }, { "filter", bsonQuery } };

        var result = await Collection.Database.RunCommandAsync<BsonDocument>(command);
        var documents = result["cursor"]["firstBatch"].AsBsonArray;

        return documents
            .Select(d =>
                MongoDB.Bson.Serialization.BsonSerializer.Deserialize<TEntity>(d.AsBsonDocument)
            )
            .ToList();
    }
}
