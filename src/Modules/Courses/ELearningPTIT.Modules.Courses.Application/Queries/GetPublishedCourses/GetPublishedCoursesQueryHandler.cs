using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetPublishedCoursesQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<GetPublishedCoursesQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> HandleAsync(GetPublishedCoursesQuery query, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.QueryAsync(x => x.Status == CourseStatus.Published, cancellationToken);
        var paged = courses
            .OrderByDescending(x => x.PublishedAt)
            .Skip(query.Skip)
            .Take(query.Take)
            .ToList();
        return paged.Adapt<List<CourseDto>>();
    }
}
