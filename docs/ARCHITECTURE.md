# Architecture Guide

This document provides a comprehensive overview of the architectural decisions, patterns, and principles used in this Onion Architecture boilerplate.

## Table of Contents

- [What is Onion Architecture?](#what-is-onion-architecture)
- [Layer Descriptions](#layer-descriptions)
- [Dependency Rules](#dependency-rules)
- [Design Patterns](#design-patterns)
- [Module Organization](#module-organization)
- [Request Flow](#request-flow)
- [Best Practices](#best-practices)

## What is Onion Architecture?

Onion Architecture (also known as Clean Architecture) is a software architectural pattern that emphasizes separation of concerns and dependency inversion. The core principle is that **dependencies should point inward** toward the domain, which represents the core business logic.

### Core Principles

1. **Domain-Centric Design**: Business logic is at the center and independent of external concerns
2. **Dependency Inversion**: Outer layers depend on inner layers, never the reverse
3. **Testability**: Easy to unit test business logic without external dependencies
4. **Technology Independence**: Business logic doesn't depend on frameworks or databases
5. **UI Independence**: Domain logic is independent of how it's presented

### Benefits

- **Maintainability**: Clear separation makes code easier to understand and modify
- **Testability**: Domain logic can be tested in isolation
- **Flexibility**: Easy to swap out infrastructure implementations
- **Scalability**: Modular design allows features to grow independently
- **Team Collaboration**: Clear boundaries enable parallel development

## Layer Descriptions

### 1. Domain Layer (Core)

**Location**: `YourCompany.YourProject.Modules.{ModuleName}.Domain`

**Purpose**: Contains the core business logic and domain models

**Responsibilities**:

- Define business entities and value objects
- Define repository interfaces (not implementations)
- Define domain services and business rules
- Contain no dependencies on other layers

**What Goes Here**:

```csharp
// Entities
public class Product : EntityBase
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }

    // Domain logic
    public void ApplyDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Invalid discount percentage");

        Price -= Price * (percentage / 100);
    }
}
```

**Dependencies**: None (only .NET standard libraries)

### 2. Application Layer

**Location**: `YourCompany.YourProject.Modules.{ModuleName}.Application`

**Purpose**: Implements business use cases and orchestrates domain objects

**Responsibilities**:

- Define commands (write operations) and queries (read operations)
- Implement command/query handlers using Wemogy CQRS
- Orchestrate domain entities and services
- Define application-specific interfaces

**What Goes Here**:

```csharp
// Commands
public record CreateProductCommand : ICommand<Product>
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}

// Command Handlers
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Price = command.Price
        };

        return await _repository.AddAsync(product, cancellationToken);
    }
}

// Queries
public record GetProductQuery : IQuery<Product?>
{
    public required Guid Id { get; init; }
}

// Query Handlers
public class GetProductQueryHandler : IQueryHandler<GetProductQuery, Product?>
{
    private readonly IProductRepository _repository;

    public GetProductQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product?> HandleAsync(GetProductQuery query, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(query.Id, cancellationToken);
    }
}
```

**Dependencies**: Domain layer only

### 3. Infrastructure Layer

**Location**: `YourCompany.YourProject.Modules.{ModuleName}.Infrastructure`

**Purpose**: Implements interfaces defined in Domain/Application layers

**Responsibilities**:

- Implement repository interfaces
- Database context and configurations
- External service integrations
- File system access
- Caching implementations

**What Goes Here**:

```csharp
// Repository Implementation
public class InMemoryRepository<TEntity> : IRepository<TEntity>
    where TEntity : IEntityBase
{
    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TEntity>>(Entities);
    }

    //... Other repository methods
}

// Dependency Injection
public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
{
    // Register repositories for specific entities
    services.AddRepository<Product>();
    return services;
}
```

**Dependencies**: Domain and Application layers

### 4. API Layer (Presentation)

**Location**: `YourCompany.YourProject.Modules.{ModuleName}.Api`

**Purpose**: Exposes HTTP endpoints and handles web requests

**Responsibilities**:

- Define FastEndpoints
- Define DTOs (Data Transfer Objects)
- Map between domain models and DTOs
- Handle HTTP concerns (status codes, validation)
- API versioning and routing

**What Goes Here**:

```csharp
// DTOs
public class ProductDto : EntityBaseDto
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}

// Request Models
public class CreateProductRequest
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}

// Endpoints
public class CreateProductEndpoint : EndpointBase<CreateProductRequest, ProductDto>
{
    private readonly ICommands _commands;

    public CreateProductEndpoint(ICommands commands)
    {
        _commands = commands;
    }

    public override void Configure()
    {
        Post("/products");
        Version(1);
        Group<ProductGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var command = new CreateProductCommand
        {
            Name = req.Name,
            Price = req.Price
        };

        var result = await _commands.RunAsync(command);
        var dto = result.Adapt<ProductDto>();

        await SendCreatedAsync(dto, ct);
    }
}
```

**Dependencies**: Application layer

## Dependency Rules

### Allowed Dependencies

```
Domain Layer:
  ├── No dependencies (pure business logic)

Application Layer:
  ├── Domain Layer ✓

Infrastructure Layer:
  ├── Domain Layer ✓
  ├── Application Layer ✓

API Layer:
  ├── Application Layer ✓
  └── Domain Layer (for DTOs/mapping only) ✓
```

### Forbidden Dependencies

- Domain → Application ✗
- Domain → Infrastructure ✗
- Domain → API ✗
- Application → Infrastructure ✗
- Application → API ✗

### Dependency Inversion in Practice

Instead of Application depending on Infrastructure:

```csharp
// ✗ BAD - Direct dependency
public class CreateProductHandler
{
    private readonly SqlProductRepository _repo; // Infrastructure dependency!
}

// ✓ GOOD - Depend on abstraction
public class CreateProductHandler
{
    private readonly IProductRepository _repo; // Domain interface
}
```

The Infrastructure layer implements the interface, and DI wires it up at runtime.

## Design Patterns

### 1. CQRS (Command Query Responsibility Segregation)

Separate read operations (Queries) from write operations (Commands).

**Benefits**:

- Optimized read and write models
- Scalability (can scale reads and writes independently)
- Clear intent (commands change state, queries don't)

**Implementation**:

```csharp
// Command - changes state
public record CreateProductCommand : ICommand<Product> { }

// Query - reads state
public record GetProductQuery : IQuery<Product> { }
```

### 2. CQRS with ICommands and IQueries

Use Wemogy CQRS `ICommands` and `IQueries` interfaces to execute commands and queries.

**Benefits**:

- Reduced coupling between components
- Single Responsibility Principle
- Easy to add cross-cutting concerns (logging, validation)

**Implementation**:

```csharp
// In endpoint - inject ICommands for write operations
public class CreateProductEndpoint : EndpointBase<CreateProductRequest, ProductDto>
{
    private readonly ICommands _commands;

    public CreateProductEndpoint(ICommands commands)
    {
        _commands = commands;
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var command = new CreateProductCommand { Name = req.Name, Price = req.Price };
        var result = await _commands.RunAsync(command);
        // ...
    }
}

// In endpoint - inject IQueries for read operations
public class GetProductEndpoint : EndpointBase<GetProductRequest, ProductDto>
{
    private readonly IQueries _queries;

    public GetProductEndpoint(IQueries queries)
    {
        _queries = queries;
    }

    public override async Task HandleAsync(GetProductRequest req, CancellationToken ct)
    {
        var query = new GetProductQuery { Id = req.Id };
        var result = await _queries.QueryAsync(query, ct);
        // ...
    }
}
```

### 3. Repository Pattern

Abstract data access logic behind repository interfaces.

**Benefits**:

- Testability (easy to mock repositories)
- Flexibility (swap implementations)
- Centralized data access logic

**Implementation**:

```csharp
// Interface in Domain
public interface IRepository<TEntity> where TEntity : IEntityBase
{
    Task<TEntity?> GetByIdAsync(string id, CancellationToken ct);
}

// Implementation in Infrastructure
public abstract class RepositoryImplementation<IEntity> : IRepository<TEntity> where TEntity : IEntityBase
{
    // EF Core, Dapper, or any other implementation
}
```

### 4. Dependency Injection

Use constructor injection for all dependencies.

**Benefits**:

- Loose coupling
- Testability
- Inversion of Control

**Implementation**:

```csharp
public class CreateProductEndpoint : EndpointBase<CreateProductRequest, ProductDto>
{
    private readonly ICommands _commands;

    // Dependencies injected via constructor
    public CreateProductEndpoint(ICommands commands)
    {
        _commands = commands;
    }
}
```

## Module Organization

### Modular Monolith

This boilerplate uses a modular monolith approach:

- Each feature is a **self-contained module**
- Modules communicate through well-defined interfaces
- Can be extracted to microservices later if needed

### Module Structure

```
YourFeature/
├── Api/                    # HTTP layer
│   ├── Endpoints/
│   ├── Dtos/
│   └── DependencyInjection.cs
├── Application/            # Use cases
│   ├── Commands/
│   ├── Queries/
│   └── DependencyInjection.cs
├── Domain/                 # Business logic
│   ├── Entities/
│   └── Repositories/
├── Infrastructure/         # Data access
│   ├── Repositories/
│   └── DependencyInjection.cs
└── Tests/
    ├── UnitTests/
    └── IntegrationTests/
```

### Shared Layer

Common functionality used across modules:

```
Shared/
├── Api/                    # Shared API abstractions (EndpointBase)
├── Core/                   # Shared utilities (Result types, extensions)
└── Infrastructure/         # Shared infrastructure (DbContext base, caching)
```

## Request Flow

### Typical HTTP Request Flow

```
1. HTTP Request
   ↓
2. FastEndpoint receives request
   ↓
3. Maps Request → Command/Query
   ↓
4. Sends to CQRS command/query runner
   ↓
5. CQRS dispatches to Handler
   ↓
6. Handler uses Domain entities
   ↓
7. Handler calls Repository (interface)
   ↓
8. Repository (Infrastructure) accesses data
   ↓
9. Returns Domain entity
   ↓
10. Handler returns result
    ↓
11. Endpoint maps Domain → DTO
    ↓
12. HTTP Response
```

### Example: Create Product Flow

```csharp
// 1. HTTP POST /api/v1/products
POST /api/v1/products
{ "name": "Widget", "price": 9.99 }

// 2. CreateProductEndpoint receives request
public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
{
    // 3. Map to Command
    var command = new CreateProductCommand
    {
        Name = req.Name,
        Price = req.Price
    };

    // 4-5. Run command via ICommands
    var product = await _commands.RunAsync(command);

    // 11. Map to DTO
    var dto = product.Adapt<ProductDto>();

    // 12. Return response
    await SendCreatedAsync(dto, ct);
}

// 6-9. Handler processes request
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _repository;

    public async Task<Product> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product { Name = command.Name, Price = command.Price };
        return await _repository.AddAsync(product, cancellationToken);
    }
}
```

## Best Practices

### 1. Keep Domain Pure

```csharp
// ✓ GOOD - Pure domain logic
public class Product : EntityBase
{
    public void ApplyDiscount(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Invalid percentage");
        Price -= Price * (percentage / 100);
    }
}

// ✗ BAD - Infrastructure concerns in domain
public class Product : EntityBase
{
    public async Task SaveToDatabase() // ✗ Don't do this!
    {
        // Database logic doesn't belong here
    }
}
```

### 2. Use Value Objects for Domain Concepts

```csharp
// ✓ GOOD - Encapsulates validation and behavior
public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0) throw new ArgumentException("Amount must be positive");
        if (string.IsNullOrEmpty(currency)) throw new ArgumentException("Currency required");

        Amount = amount;
        Currency = currency;
    }
}
```

### 3. Keep Handlers Thin

```csharp
// ✓ GOOD - Handler orchestrates, domain does work
public class ApplyDiscountHandler : ICommandHandler<ApplyDiscountCommand, Product>
{
    public Task<Product> HandleAsync(ApplyDiscountCommand request, CancellationToken ct)
    {
        var product = await _repository.GetAsync(request.ProductId, ct);
        product.ApplyDiscount(request.Percentage); // Domain logic
        return _repository.UpdateAsync(product, ct);
    }
}
```

### 4. Use DTOs for External Communication

```csharp
// ✓ GOOD - Separate DTO from domain model
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// ✗ BAD - Exposing domain entities directly
public async Task<Product> GetProduct(Guid id) // ✗ Don't return domain entities from API
```

### 5. Validate at Boundaries

```csharp
// API Layer - Input validation
public class CreateProductRequest
{
    [Required]
    [MinLength(3)]
    public required string Name { get; set; }

    [Range(0.01, double.MaxValue)]
    public required decimal Price { get; set; }
}

// Domain Layer - Business rule validation
public class Product
{
    public void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new DomainException("Price must be positive");
        Price = price;
    }
}
```

### 6. Use Meaningful Names

```csharp
// ✓ GOOD
public class CreateProductCommand
public class CreateProductCommandHandler
public class GetProductQuery
public class GetProductQueryHandler

// ✗ BAD
public class Command1
public class Handler
public class ProductService // Too generic
```

### 7. Keep Module Boundaries Clear

```csharp
// ✓ GOOD - Modules communicate through public interfaces
public interface IOrderService
{
    Task<Order> CreateOrder(CreateOrderCommand command);
}

// ✗ BAD - Direct access to other module's internals
var product = _otherModule.InternalRepository.Get(id); // ✗ Breaks encapsulation
```

## Testing Strategy

### Unit Tests

- Test domain logic in isolation
- Test handlers with mocked repositories
- Fast, no external dependencies

### Integration Tests

- Test API endpoints end-to-end
- Use in-memory database or test containers
- Verify full request flow

### Example

```csharp
// Unit Test - Domain
[Fact]
public void ApplyDiscount_WithValidPercentage_ReducesPrice()
{
    var product = new Product { Name = "Test", Price = 100 };
    product.ApplyDiscount(10);
    Assert.Equal(90, product.Price);
}

// Integration Test - Endpoint
[Fact]
public async Task CreateProduct_ReturnsCreated()
{
    var response = await _client.PostAsync("/api/v1/products",
        new { name = "Test", price = 9.99 });
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
}
```

## Conclusion

This Onion Architecture boilerplate provides a solid foundation for building maintainable, testable, and scalable applications. By following these architectural principles and patterns, you can build systems that are:

- Easy to understand and navigate
- Simple to test at all levels
- Flexible and adaptable to changing requirements
- Ready to scale as your business grows

For more details, see:

- [Getting Started Guide](GETTING_STARTED.md)
- [Module Creation Guide](MODULE_CREATION.md)
- [Best Practices](BEST_PRACTICES.md)
