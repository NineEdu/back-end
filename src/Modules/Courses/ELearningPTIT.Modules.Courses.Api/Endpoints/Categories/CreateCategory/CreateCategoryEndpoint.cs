using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Categories.CreateCategory;

public class CreateCategoryEndpoint(
    Wemogy.CQRS.Commands.Abstractions.ICommandHandler<CreateCategoryCommand, CategoryDto> commandHandler)
    : Endpoint<CreateCategoryRequest, CategoryDto>
{
    public override void Configure()
    {
        Post("/");
        Group<CategoriesGroup>();
        // TODO: Add permission-based authorization: categories:create (Admin only)
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
    {
        var command = new CreateCategoryCommand
        {
            Name = req.Name,
            Description = req.Description,
            Icon = req.Icon,
            ParentCategoryId = req.ParentCategoryId,
            DisplayOrder = req.DisplayOrder
        };

        var result = await commandHandler.HandleAsync(command);
        await Send.ResponseAsync(result, 201, ct);
    }
}
