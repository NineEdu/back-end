# FastEndpoints Guide

This project uses [FastEndpoints](https://fast-endpoints.com/) for building API endpoints with minimal overhead and maximum performance.

## Project Structure

Endpoints are organized by feature in the `Modules` folder:

```
src/Modules/YourFeature/
└── YourCompany.YourProject.Modules.YourFeature.Api/
    └── Endpoints/
        └── Products/
            ├── ProductGroup.cs
            ├── CreateProduct/
            │   ├── CreateProductEndpoint.cs
            │   └── CreateProductRequest.cs
            └── GetProduct/
                ├── GetProductEndpoint.cs
                └── GetProductRequest.cs
```

## Creating a New Endpoint

### 1. Create Request and Endpoint Files

Create a folder for your endpoint under `Endpoints/{Entity}/{Action}/`:

```csharp
// CreateProductRequest.cs
namespace YourCompany.YourProject.Modules.YourFeature.Api.Endpoints.Products.CreateProduct;

public class CreateProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

```csharp
// CreateProductEndpoint.cs
using Wemogy.CQRS.Commands.Abstractions;
using YourCompany.YourProject.Shared.Api.Abstractions;

namespace YourCompany.YourProject.Modules.YourFeature.Api.Endpoints.Products.CreateProduct;

public class CreateProductEndpoint(ICommands commands)
    : EndpointBase<CreateProductRequest, ProductDto>
{
    public override void Configure()
    {
        Post("/products");
        Version(1);
        Group<ProductGroup>();
        AllowAnonymous(); // or add authorization
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var command = new CreateProductCommand
        {
            Name = req.Name,
            Price = req.Price
        };

        var result = await commands.RunAsync(command);
        var dto = result.Adapt<ProductDto>();
        await SendCreatedAsync(dto, ct);
    }
}
```

### 2. Create a Group (for Swagger organization)

```csharp
// ProductGroup.cs
using FastEndpoints;

namespace YourCompany.YourProject.Modules.YourFeature.Api.Endpoints.Products;

public sealed class ProductGroup : Group
{
    public ProductGroup()
    {
        Configure("products", ep =>
        {
            ep.Description(x => x.WithTags("Products"));
        });
    }
}
```

## EndpointBase Helper Methods

The `EndpointBase<TRequest, TResponse>` class provides convenient response methods:

- `SendCreatedAsync(response, ct)` - Returns 201 Created
- `SendAcceptedAsync(response, ct)` - Returns 202 Accepted
- `SendOkAsync(response, ct)` - Returns 200 OK
- `SendNoContentAsync(ct)` - Returns 204 No Content
- `SendUnauthorizedAsync(ct)` - Returns 401 Unauthorized
- `SendBadRequestAsync(ct)` - Returns 400 Bad Request
- `SendForbiddenAsync(ct)` - Returns 403 Forbidden
- `SendNotFoundAsync(ct)` - Returns 404 Not Found
- `SendErrorAsync(statusCode, ct)` - Returns custom error status code

## Configuration Options

### Endpoint Configuration

In the `Configure()` method, you can set:

```csharp
public override void Configure()
{
    Post("/products");           // HTTP method and route
    Version(1);                   // API version
    Group<ProductGroup>();        // Swagger group/tag

    // Authorization
    AllowAnonymous();            // No auth required
    // or
    Policies("AdminPolicy");      // Require policy
    // or
    Roles("Admin", "Manager");    // Require roles

    // Additional options
    Description(x => x
        .WithName("CreateProduct")
        .WithSummary("Creates a new product")
        .Produces<ProductDto>(201)
        .ProducesProblemFE(400));
}
```

### Program.cs Setup

```csharp
// Add FastEndpoints and Swagger
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "Your API";
    config.Version = "v1.0";
});

var app = builder.Build();

// Use FastEndpoints and Swagger
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseFastEndpoints()
   .UseSwaggerGen();
```

## Common Patterns

### GET with Route Parameters

```csharp
public class GetProductRequest
{
    public Guid Id { get; set; }
}

public override void Configure()
{
    Get("/products/{id}");
    Version(1);
}
```

### Query Parameters

```csharp
public class SearchProductsRequest
{
    [QueryParam]
    public string? SearchTerm { get; set; }

    [QueryParam]
    public int Page { get; set; } = 1;
}
```

### Validation

Add a validator class:

```csharp
public class CreateProductValidator : Validator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}
```

## Resources

- [FastEndpoints Documentation](https://fast-endpoints.com/)
- [FastEndpoints GitHub](https://github.com/FastEndpoints/FastEndpoints)
