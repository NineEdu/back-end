namespace YourCompany.YourProject.Modules.YourFeature.Api.Endpoints.Products.CreateProduct;

public class CreateProductRequest
{
    public required string TenantId { get; set; }

    public required string Name { get; set; }

    public required decimal Price { get; set; }
}
