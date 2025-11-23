using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository
) : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> HandleAsync(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = query.ActiveOnly
            ? await categoryRepository.QueryAsync(x => x.IsActive, cancellationToken)
            : await categoryRepository.GetAllAsync(cancellationToken);

        return categories.OrderBy(x => x.DisplayOrder).Adapt<List<CategoryDto>>();
    }
}
