using Wemogy.CQRS.Queries.Abstractions;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.Modules.YourFeature.Application.Queries.Products.GetProduct;

public class GetProductQuery : IQuery<Product>
{
    public required string Id { get; init; }
}
