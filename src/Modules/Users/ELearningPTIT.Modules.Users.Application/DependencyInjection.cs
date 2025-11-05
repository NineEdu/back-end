using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Wemogy.CQRS;

namespace ELearningPTIT.Modules.Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register CQRS
        services.AddCQRS();

        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
