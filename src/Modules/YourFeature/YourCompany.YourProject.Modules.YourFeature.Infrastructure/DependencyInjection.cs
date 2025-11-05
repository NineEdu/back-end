using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Shared.Core;
using YourCompany.YourProject.Shared.Core.Options;
using YourCompany.YourProject.Shared.Infrastructure.CosmosDb;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.Modules.YourFeature.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        CoreSetupOptions options
    )
    {
        // Register repositories for specific entities
        services.AddCosmosDbRepository<Product>(
            options.DatabaseSetupOptions.ConnectionString!,
            options.DatabaseSetupOptions.DatabaseName!
        );
        return services;
    }
}
