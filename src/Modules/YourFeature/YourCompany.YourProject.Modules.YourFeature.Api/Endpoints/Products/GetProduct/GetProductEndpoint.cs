using FastEndpoints;
using Mapster;
using Wemogy.CQRS.Queries.Abstractions;
using YourCompany.YourProject.Modules.YourFeature.Api.Dtos;
using YourCompany.YourProject.Modules.YourFeature.Application.Queries.Products.GetProduct;

namespace YourCompany.YourProject.Modules.YourFeature.Api.Endpoints.Products.GetProduct;

public class GetProductEndpoint(IQueries queries) : Endpoint<GetProductRequest, ProductDto>
{
    public override void Configure()
    {
        Get("/{id}");
        Group<ProductGroup>();

        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductRequest req, CancellationToken ct)
    {
        var command = new GetProductQuery() { Id = req.Id };

        var result = await queries.QueryAsync(command, ct);
        var dto = result.Adapt<ProductDto>();
        await Send.OkAsync(dto, ct);
    }
}
