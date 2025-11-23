using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class SearchCoursesQueryHandler(
    ICourseRepository courseRepository
) : IQueryHandler<SearchCoursesQuery, List<CourseDto>>
{
    public async Task<List<CourseDto>> HandleAsync(SearchCoursesQuery query, CancellationToken cancellationToken)
    {
        var searchTerm = query.SearchTerm.ToLowerInvariant();
        var courses = await courseRepository.QueryAsync(
            x => x.Status == CourseStatus.Published &&
                 (x.Title.ToLower().Contains(searchTerm) || (x.Description != null && x.Description.ToLower().Contains(searchTerm))),
            cancellationToken);
        return courses.Take(50).Adapt<List<CourseDto>>();
    }
}
