using System.Linq.Expressions;
using YourCompany.YourProject.Shared.Core.Abstractions;

namespace YourCompany.YourProject.Shared.Infrastructure.Repositories;

public class InMemoryRepository<TEntity> : IRepository<TEntity>
    where TEntity : IEntityBase
{
    private static readonly List<TEntity> Entities = new();

    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TEntity>>(Entities);
    }

    public Task UpdateAsync(TEntity entity)
    {
        var existingTEntity = Entities.FirstOrDefault(p => p.Id == entity.Id);
        if (existingTEntity != null)
        {
            Entities.Remove(existingTEntity);
            Entities.Add(entity);
        }
        return Task.CompletedTask;
    }

    Task<TEntity> IRepository<TEntity>.CreateAsync(TEntity entity)
    {
        Entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(string id)
    {
        var entity = Entities.FirstOrDefault(p => p.Id == id);
        if (entity != null)
        {
            Entities.Remove(entity);
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entitiesToDelete = Entities.AsQueryable().Where(predicate).ToList();
        foreach (var entity in entitiesToDelete)
        {
            Entities.Remove(entity);
        }
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default)
    {
        var exists = Entities.Any(p => p.Id == id);
        return Task.FromResult(exists);
    }

    public Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var exists = Entities.AsQueryable().Any(predicate);
        return Task.FromResult(exists);
    }

    public Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = Entities.FirstOrDefault(p => p.Id == id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"TEntity with ID {id} not found.");
        }
        return Task.FromResult(entity);
    }

    public Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var entity = Entities.AsQueryable().FirstOrDefault(predicate);
        if (entity == null)
        {
            throw new KeyNotFoundException("TEntity not found.");
        }
        return Task.FromResult(entity);
    }

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Entities.ToList());
    }

    public Task<List<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var entities = Entities.AsQueryable().Where(predicate).ToList();
        return Task.FromResult(entities);
    }

    public Task<TEntity> ReplaceAsync(TEntity entity)
    {
        var existingTEntity = Entities.FirstOrDefault(p => p.Id == entity.Id);
        if (existingTEntity != null)
        {
            Entities.Remove(existingTEntity);
        }
        Entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<TEntity> UpdateAsync(string id, Action<TEntity> updateAction)
    {
        var entity = Entities.FirstOrDefault(p => p.Id == id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"TEntity with ID {id} not found.");
        }

        Entities.Remove(entity);
        updateAction(entity);
        Entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<TEntity> UpdateAsync(string id, Func<TEntity, Task> updateAction)
    {
        var entity = Entities.FirstOrDefault(p => p.Id == id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"TEntity with ID {id} not found.");
        }

        Entities.Remove(entity);
        updateAction(entity).GetAwaiter().GetResult();
        Entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<long> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    )
    {
        var count = Entities.AsQueryable().Count(predicate);
        return Task.FromResult((long)count);
    }

    public Task<TEntity> UpsertAsync(TEntity entity)
    {
        var existingTEntity = Entities.FirstOrDefault(p => p.Id == entity.Id);
        if (existingTEntity != null)
        {
            Entities.Remove(existingTEntity);
        }
        Entities.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<List<TEntity>> ExecuteRawQueryAsync(string query, params object[] parameters)
    {
        // In-memory repository does not support raw queries.
        throw new NotImplementedException(
            "Raw query execution is not supported in InMemoryRepository."
        );
    }
}
