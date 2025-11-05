using YourCompany.YourProject.Shared.Api.Abstractions;

namespace YourCompany.YourProject.Modules.YourFeature.Api.Dtos;

public class ProductDto : EntityBaseDto
{
    public required string Name { get; set; }

    public required decimal Price { get; set; }
}
