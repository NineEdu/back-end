using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb.Repositories;

namespace YourCompany.YourProject.Shared.Infrastructure.MongoDb;

public static class DependencyInjection
{
    /// <summary>
    /// Registers a MongoDB repository for the specified entity type.
    /// </summary>
    public static IServiceCollection AddMongoDbRepository<TEntity>(
        this IServiceCollection services,
        string databaseName
    )
        where TEntity : class, IEntityBase
    {
        services.AddScoped<IRepository<TEntity>>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var database = mongoClient.GetDatabase(databaseName);
            return new MongoDbRepository<TEntity>(database);
        });
        return services;
    }

    /// <summary>
    /// Registers a custom MongoDB repository implementation.
    /// </summary>
    public static IServiceCollection AddMongoDbRepository<TEntity, TRepository>(
        this IServiceCollection services
    )
        where TEntity : class, IEntityBase
        where TRepository : class, IRepository<TEntity>
    {
        services.AddScoped<IRepository<TEntity>, TRepository>();
        return services;
    }

    /// <summary>
    /// Registers the MongoDB client as a singleton.
    /// </summary>
    public static IServiceCollection AddMongoDbClient(
        this IServiceCollection services,
        string connectionString
    )
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
        return services;
    }

    /// <summary>
    /// Registers the MongoDB client as a singleton with custom settings.
    /// </summary>
    public static IServiceCollection AddMongoDbClient(
        this IServiceCollection services,
        MongoClientSettings settings
    )
    {
        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings));
        return services;
    }
}
