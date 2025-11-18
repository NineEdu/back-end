using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCategoryByIdQueryHandler(
    ICategoryRepository categoryRepository
) : IQueryHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> HandleAsync(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetAsync(query.CategoryId, cancellationToken);
        return category?.Adapt<CategoryDto>();
    }
}
