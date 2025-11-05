using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YourCompany.YourProject.Shared.Core.Abstractions;

namespace YourCompany.YourProject.Shared.Infrastructure.EntityFramework.Repositories;

/// <summary>
/// Generic Entity Framework Core repository implementation.
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TContext">The DbContext type</typeparam>
public class EfRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class, IEntityBase
    where TContext : DbContext
{
    protected readonly TContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public EfRepository(TContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> GetAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await DbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }
        return entity;
    }

    public virtual async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var entity = await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        if (entity == null)
        {
            throw new KeyNotFoundException("Entity not found.");
        }
        return entity;
    }

    public virtual async Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.AnyAsync(e => e.Id == id, cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<long> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        return await DbSet.LongCountAsync(predicate, cancellationToken);
    }

    public virtual async Task DeleteAsync(string id)
    {
        var entity = await GetAsync(id);
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await DbSet.Where(predicate).ToListAsync();
        DbSet.RemoveRange(entities);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<TEntity> ReplaceAsync(TEntity entity)
    {
        var existingEntity = await DbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            Context.Entry(existingEntity).State = EntityState.Detached;
        }

        DbSet.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(string id, Action<TEntity> updateAction)
    {
        var entity = await GetAsync(id);
        updateAction(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(string id, Func<TEntity, Task> updateAction)
    {
        var entity = await GetAsync(id);
        await updateAction(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpsertAsync(TEntity entity)
    {
        var exists = await ExistsAsync(entity.Id);
        if (exists)
        {
            return await ReplaceAsync(entity);
        }
        return await CreateAsync(entity);
    }

    public Task<List<TEntity>> ExecuteRawQueryAsync(string sql, params object[] parameters)
    {
        return DbSet.FromSqlRaw(sql, parameters).ToListAsync();
    }
}
