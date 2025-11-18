using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetPublishedCoursesQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<GetPublishedCoursesQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> HandleAsync(GetPublishedCoursesQuery query, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.GetPublishedCoursesAsync(query.Skip, query.Take, cancellationToken);
        return courses.Adapt<List<CourseDto>>();
    }
}
