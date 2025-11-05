using Microsoft.Extensions.DependencyInjection;
using Wemogy.CQRS;

namespace YourCompany.YourProject.Modules.YourFeature.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers all services required by the YourFeature module
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register CQRS
        services.AddCQRS();

        return services;
    }
}
