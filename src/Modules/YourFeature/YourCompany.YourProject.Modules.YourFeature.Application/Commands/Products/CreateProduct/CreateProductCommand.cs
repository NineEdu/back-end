using Wemogy.CQRS.Commands.Abstractions;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.Modules.YourFeature.Application.Commands.Products.CreateProduct;

public class CreateProductCommand : ICommand<Product>
{
    public required string TenantId { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}
