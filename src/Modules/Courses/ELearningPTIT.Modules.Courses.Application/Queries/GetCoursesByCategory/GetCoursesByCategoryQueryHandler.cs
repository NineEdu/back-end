using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCoursesByCategoryQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<GetCoursesByCategoryQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> HandleAsync(GetCoursesByCategoryQuery query, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.QueryAsync(x => x.CategoryId == query.CategoryId, cancellationToken);
        return courses.Adapt<List<CourseDto>>();
    }
}
