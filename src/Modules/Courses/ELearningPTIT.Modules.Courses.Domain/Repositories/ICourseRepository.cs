using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using YourCompany.YourProject.Shared.Core.Abstractions;

namespace ELearningPTIT.Modules.Courses.Domain.Repositories;

/// <summary>
/// Repository interface for Course entity
/// </summary>
public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Course>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Course>> GetByStatusAsync(CourseStatus status, CancellationToken cancellationToken = default);

    Task<Course?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<IEnumerable<Course>> GetPublishedCoursesAsync(int skip = 0, int take = 20, CancellationToken cancellationToken = default);

    Task<IEnumerable<Course>> SearchCoursesAsync(string searchTerm, CancellationToken cancellationToken = default);

    Task UpdateRatingAsync(string courseId, double averageRating, int totalReviews, CancellationToken cancellationToken = default);

    Task IncrementEnrollmentCountAsync(string courseId, CancellationToken cancellationToken = default);
}
