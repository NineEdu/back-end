using ELearningPTIT.Modules.Media.Application.Abstractions;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using ELearningPTIT.Modules.Media.Infrastructure.Configuration;
using ELearningPTIT.Modules.Media.Infrastructure.Repositories;
using ELearningPTIT.Modules.Media.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ELearningPTIT.Modules.Media.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<AzureBlobStorageSettings>(
            configuration.GetSection("AzureBlobStorage"));

        services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

        services.AddScoped<IMediaAssetRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
                ?? throw new InvalidOperationException("MongoDB database name is not configured");
            var database = mongoClient.GetDatabase(databaseName);
            return new MediaAssetRepository(database);
        });

        services.AddScoped<ITranscodingJobRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
                ?? throw new InvalidOperationException("MongoDB database name is not configured");
            var database = mongoClient.GetDatabase(databaseName);
            return new TranscodingJobRepository(database);
        });

        return services;
    }
}
