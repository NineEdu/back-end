using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCourseReviewsQueryHandler(
    IReviewRepository reviewRepository
) : IQueryHandler<GetCourseReviewsQuery, List<ReviewDto>>
{
    public async Task<List<ReviewDto>> HandleAsync(GetCourseReviewsQuery query, CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.QueryAsync(
            x => x.CourseId == query.CourseId && x.IsPublished,
            cancellationToken);
        return reviews.OrderByDescending(x => x.CreatedAt).Adapt<List<ReviewDto>>();
    }
}
