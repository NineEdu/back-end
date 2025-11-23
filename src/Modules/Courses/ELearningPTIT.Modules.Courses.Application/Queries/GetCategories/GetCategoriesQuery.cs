using ELearningPTIT.Modules.Courses.Application.DTOs;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Queries;

public class GetCategoriesQuery : IQuery<List<CategoryDto>>
{
    public bool ActiveOnly { get; set; } = true;
}
