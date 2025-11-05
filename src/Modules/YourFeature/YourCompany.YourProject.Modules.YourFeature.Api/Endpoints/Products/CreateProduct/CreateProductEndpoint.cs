using FastEndpoints;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;
using YourCompany.YourProject.Modules.YourFeature.Api.Dtos;
using YourCompany.YourProject.Modules.YourFeature.Application.Commands.Products.CreateProduct;

namespace YourCompany.YourProject.Modules.YourFeature.Api.Endpoints.Products.CreateProduct;

public class CreateProductEndpoint(ICommands commands) : Endpoint<CreateProductRequest, ProductDto>
{
    public override void Configure()
    {
        Post("/");
        Group<ProductGroup>();

        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var command = new CreateProductCommand
        {
            TenantId = req.TenantId,
            Name = req.Name,
            Price = req.Price,
        };

        var result = await commands.RunAsync(command);
        var dto = result.Adapt<ProductDto>();
        await Send.ResponseAsync(dto, 201, ct);
    }
}
