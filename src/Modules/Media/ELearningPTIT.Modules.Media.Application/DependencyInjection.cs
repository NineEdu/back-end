using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Wemogy.CQRS;

namespace ELearningPTIT.Modules.Media.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddCQRS();

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
