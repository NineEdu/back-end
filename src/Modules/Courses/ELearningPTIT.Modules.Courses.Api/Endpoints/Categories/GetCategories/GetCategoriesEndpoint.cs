using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Application.Queries;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Categories.GetCategories;

public class GetCategoriesEndpoint(
    IQueryHandler<GetCategoriesQuery, List<CategoryDto>> queryHandler)
    : EndpointWithoutRequest<List<CategoryDto>>
{
    public override void Configure()
    {
        Get("/");
        Group<CategoriesGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = new GetCategoriesQuery { ActiveOnly = true };
        var result = await queryHandler.HandleAsync(query, ct);
        await Send.ResponseAsync(result, 200, ct);
    }
}
