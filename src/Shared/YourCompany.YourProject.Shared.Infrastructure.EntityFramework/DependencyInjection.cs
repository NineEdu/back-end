using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Infrastructure.EntityFramework.Repositories;

namespace YourCompany.YourProject.Shared.Infrastructure.EntityFramework;

public static class DependencyInjection
{
    /// <summary>
    /// Registers an Entity Framework Core repository for the specified entity type.
    /// </summary>
    public static IServiceCollection AddEfRepository<TEntity, TContext>(
        this IServiceCollection services
    )
        where TEntity : class, IEntityBase
        where TContext : DbContext
    {
        services.AddScoped<IRepository<TEntity>, EfRepository<TEntity, TContext>>();
        return services;
    }
}
