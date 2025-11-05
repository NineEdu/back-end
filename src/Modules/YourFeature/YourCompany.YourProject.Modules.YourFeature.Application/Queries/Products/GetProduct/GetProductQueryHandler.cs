using Wemogy.CQRS.Queries.Abstractions;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.Modules.YourFeature.Application.Queries.Products.GetProduct;

public class GetProductQueryHandler(IRepository<Product> productRepository)
    : IQueryHandler<GetProductQuery, Product>
{
    public async Task<Product> HandleAsync(
        GetProductQuery query,
        CancellationToken cancellationToken
    )
    {
        var sqlQuery = "SELECT * FROM c WHERE c.id = @param0";
        var results = await productRepository.ExecuteRawQueryAsync(sqlQuery, query.Id);
        return results.FirstOrDefault()
            ?? throw new KeyNotFoundException($"Product with ID {query.Id} not found.");
    }
}
