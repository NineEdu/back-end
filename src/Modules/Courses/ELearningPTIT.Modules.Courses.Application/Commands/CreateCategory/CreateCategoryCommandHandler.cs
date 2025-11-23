using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository
) : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> HandleAsync(CreateCategoryCommand command)
    {
        var slug = GenerateSlug(command.Name);

        var category = new Category
        {
            Name = command.Name,
            Slug = slug,
            Description = command.Description,
            Icon = command.Icon,
            ParentCategoryId = command.ParentCategoryId,
            DisplayOrder = command.DisplayOrder,
            IsActive = true
        };

        await categoryRepository.CreateAsync(category);

        return category.Adapt<CategoryDto>();
    }

    private static string GenerateSlug(string name)
    {
        return name.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("'", "")
            .Replace("\"", "")
            .Replace(".", "")
            .Replace(",", "");
    }
}
