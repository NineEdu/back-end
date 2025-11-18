using ELearningPTIT.Modules.Courses.Application;
using ELearningPTIT.Modules.Courses.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearningPTIT.Modules.Courses.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCoursesModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register Application layer
        services.AddCoursesApplication();

        // Register Infrastructure layer
        services.AddCoursesInfrastructure(configuration);

        // FastEndpoints are automatically registered by assembly scanning

        return services;
    }
}
