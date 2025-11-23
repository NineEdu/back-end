using ELearningPTIT.Modules.Media.Application;
using ELearningPTIT.Modules.Media.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearningPTIT.Modules.Media.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddMediaModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddApplicationServices();

        services.AddInfrastructureServices(configuration);

        return services;
    }
}
