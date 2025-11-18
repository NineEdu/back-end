using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCourseByIdQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<GetCourseByIdQuery, CourseDto?>
{
    public async Task<CourseDto?> HandleAsync(GetCourseByIdQuery query, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetAsync(query.CourseId, cancellationToken);
        return course?.Adapt<CourseDto>();
    }
}
