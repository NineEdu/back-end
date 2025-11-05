using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Options;
using YourCompany.YourProject.Shared.Infrastructure.CosmosDb.Repositories;

namespace YourCompany.YourProject.Shared.Infrastructure.CosmosDb;

public static class DependencyInjection
{
    /// <summary>
    /// Registers a Cosmos DB repository for the specified entity type.
    /// </summary>
    public static IServiceCollection AddCosmosDbRepository<TEntity>(
        this IServiceCollection services,
        CosmosClient cosmosClient,
        string databaseName
    )
        where TEntity : class, IEntityBase
    {
        services.AddScoped<IRepository<TEntity>>(_ => new CosmosDbRepository<TEntity>(
            cosmosClient,
            databaseName
        ));
        return services;
    }

    public static IServiceCollection AddCosmosDbRepository<TEntity>(
        this IServiceCollection services,
        string connectionString,
        string databaseName
    )
        where TEntity : class, IEntityBase
    {
        services.AddScoped<IRepository<TEntity>>(_ =>
        {
            var client = new CosmosClient(
                connectionString,
                clientOptions: new CosmosClientOptions
                {
                    UseSystemTextJsonSerializerWithOptions = DefaultJsonSerializerOptions.Create(),
                }
            );
            return new CosmosDbRepository<TEntity>(client, databaseName);
        });
        return services;
    }

    public static IServiceCollection AddCosmosDbRepository<TEntity>(
        this IServiceCollection services,
        Uri fullNamespace,
        string databaseName
    )
        where TEntity : class, IEntityBase
    {
        services.AddScoped<IRepository<TEntity>>(_ =>
        {
            var client = new CosmosClient(
                fullNamespace.ToString(),
                new DefaultAzureCredential(),
                clientOptions: new CosmosClientOptions
                {
                    UseSystemTextJsonSerializerWithOptions = DefaultJsonSerializerOptions.Create(),
                }
            );
            return new CosmosDbRepository<TEntity>(client, databaseName);
        });
        return services;
    }
}
