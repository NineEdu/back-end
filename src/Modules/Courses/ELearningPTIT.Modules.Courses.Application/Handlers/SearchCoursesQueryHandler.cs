using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class SearchCoursesQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<SearchCoursesQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> HandleAsync(SearchCoursesQuery query, CancellationToken cancellationToken)
    {
        var courses = await courseRepository.SearchCoursesAsync(query.SearchTerm, cancellationToken);
        return courses.Adapt<List<CourseDto>>();
    }
}
