using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Wemogy.CQRS;

namespace ELearningPTIT.Modules.Courses.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCoursesApplication(this IServiceCollection services)
    {
        // Register CQRS
        services.AddCQRS();

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
