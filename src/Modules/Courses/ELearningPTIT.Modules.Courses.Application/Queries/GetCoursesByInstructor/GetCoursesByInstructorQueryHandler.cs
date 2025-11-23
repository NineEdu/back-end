using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCoursesByInstructorQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<GetCoursesByInstructorQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> HandleAsync(GetCoursesByInstructorQuery query, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.QueryAsync(x => x.InstructorId == query.InstructorId, cancellationToken);
        return courses.Adapt<List<CourseDto>>();
    }
}
