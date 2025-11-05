using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace YourCompany.YourProject.Modules.YourFeature.Api.Endpoints.Products;

public sealed class ProductGroup : Group
{
    public ProductGroup()
    {
        Configure(
            "products",
            ep =>
            {
                ep.Description(x => x.WithTags("Products"));
            }
        );
    }
}
