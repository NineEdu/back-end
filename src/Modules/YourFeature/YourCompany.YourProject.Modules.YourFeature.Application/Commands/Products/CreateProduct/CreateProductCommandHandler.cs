using Wemogy.CQRS.Commands.Abstractions;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.Modules.YourFeature.Application.Commands.Products.CreateProduct;

public class CreateProductCommandHandler(IRepository<Product> productRepository)
    : ICommandHandler<CreateProductCommand, Product>
{
    public Task<Product> HandleAsync(CreateProductCommand command)
    {
        var product = new Product
        {
            TenantId = command.TenantId,
            Name = command.Name,
            Price = command.Price,
        };

        return productRepository.CreateAsync(product);
    }
}
