using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Infrastructure.Repositories;

namespace YourCompany.YourProject.Shared.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRepository<TEntity>(this IServiceCollection services)
        where TEntity : class, IEntityBase
    {
        services.AddScoped<IRepository<TEntity>, InMemoryRepository<TEntity>>();
        return services;
    }
}
