using ELearningPTIT.Modules.Users.Application.Abstractions;
using ELearningPTIT.Modules.Users.Domain.Repositories;
using ELearningPTIT.Modules.Users.Infrastructure.Configuration;
using ELearningPTIT.Modules.Users.Infrastructure.Repositories;
using ELearningPTIT.Modules.Users.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ELearningPTIT.Modules.Users.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configure JWT settings
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        // Register services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Register repositories
        services.AddScoped<IUserRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
                ?? throw new InvalidOperationException("MongoDB database name is not configured");
            var database = mongoClient.GetDatabase(databaseName);
            return new UserRepository(database);
        });

        return services;
    }
}
