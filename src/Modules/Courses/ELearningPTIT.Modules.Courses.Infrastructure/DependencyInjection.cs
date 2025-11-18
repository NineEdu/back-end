using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ELearningPTIT.Modules.Courses.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCoursesInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register repositories with IMongoDatabase resolution
        services.AddScoped<ICourseRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
                ?? throw new InvalidOperationException("MongoDB database name is not configured");
            var database = mongoClient.GetDatabase(databaseName);
            return new CourseRepository(database);
        });

        services.AddScoped<ICategoryRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
                ?? throw new InvalidOperationException("MongoDB database name is not configured");
            var database = mongoClient.GetDatabase(databaseName);
            return new CategoryRepository(database);
        });

        services.AddScoped<IReviewRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
                ?? throw new InvalidOperationException("MongoDB database name is not configured");
            var database = mongoClient.GetDatabase(databaseName);
            return new ReviewRepository(database);
        });

        services.AddScoped<IQuestionRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value
                ?? throw new InvalidOperationException("MongoDB database name is not configured");
            var database = mongoClient.GetDatabase(databaseName);
            return new QuestionRepository(database);
        });

        return services;
    }
}
