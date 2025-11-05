using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Modules.YourFeature.Application;
using YourCompany.YourProject.Modules.YourFeature.Infrastructure;
using YourCompany.YourProject.Shared.Core;
using YourCompany.YourProject.Shared.Core.Options;

namespace YourCompany.YourProject.Modules.YourFeature.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddYourFeatureModule(
        this IServiceCollection services,
        CoreSetupOptions options
    )
    {
        // Register application services
        services.AddApplicationServices();

        // Register infrastructure services
        services.AddInfrastructureServices(options);

        return services;
    }
}
