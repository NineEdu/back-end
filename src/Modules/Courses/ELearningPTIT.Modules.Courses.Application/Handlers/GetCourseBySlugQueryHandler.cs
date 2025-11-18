using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCourseBySlugQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<GetCourseBySlugQuery, CourseDto?>
{
    public async Task<CourseDto?> HandleAsync(GetCourseBySlugQuery query, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetBySlugAsync(query.Slug, cancellationToken);
        return course?.Adapt<CourseDto>();
    }
}
