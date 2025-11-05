using System.Linq.Expressions;

namespace YourCompany.YourProject.Shared.Core.Abstractions;

public interface IRepository<TEntity>
    where TEntity : IEntityBase
{
    Task<long> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> CreateAsync(TEntity entity);

    Task DeleteAsync(string id);

    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> GetAsync(string id, CancellationToken cancellationToken = default);

    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> QueryAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> ReplaceAsync(TEntity entity);

    Task<TEntity> UpdateAsync(string id, Action<TEntity> updateAction);

    Task<TEntity> UpdateAsync(string id, Func<TEntity, Task> updateAction);

    Task<TEntity> UpsertAsync(TEntity entity);

    Task<List<TEntity>> ExecuteRawQueryAsync(string query, params object[] parameters);
}
