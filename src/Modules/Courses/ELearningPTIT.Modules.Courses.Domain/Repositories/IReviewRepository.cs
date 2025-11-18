using ELearningPTIT.Modules.Courses.Domain.Entities;
using YourCompany.YourProject.Shared.Core.Abstractions;

namespace ELearningPTIT.Modules.Courses.Domain.Repositories;

/// <summary>
/// Repository interface for Review entity
/// </summary>
public interface IReviewRepository : IRepository<Review>
{
    Task<IEnumerable<Review>> GetByCourseIdAsync(string courseId, CancellationToken cancellationToken = default);

    Task<Review?> GetByUserAndCourseAsync(string userId, string courseId, CancellationToken cancellationToken = default);

    Task<(double AverageRating, int TotalReviews)> GetCourseRatingStatsAsync(string courseId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Review>> GetFlaggedReviewsAsync(CancellationToken cancellationToken = default);
}
