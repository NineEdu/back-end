using ELearningPTIT.Modules.Courses.Domain.Entities;
using YourCompany.YourProject.Shared.Core.Abstractions;

namespace ELearningPTIT.Modules.Courses.Domain.Repositories;

/// <summary>
/// Repository interface for Category entity
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IEnumerable<Category>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Category>> GetByParentIdAsync(string? parentId, CancellationToken cancellationToken = default);

    Task IncrementCourseCountAsync(string categoryId, CancellationToken cancellationToken = default);

    Task DecrementCourseCountAsync(string categoryId, CancellationToken cancellationToken = default);
}
