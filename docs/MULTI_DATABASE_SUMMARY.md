# Multi-Database Implementation - Quick Summary

## What Was Implemented

This boilerplate now supports **four different database types**, all using the same `IRepository<TEntity>` interface:

### 1. **In-Memory Repository** (Default)

- Location: `Shared.Infrastructure/Repositories/InMemoryRepository.cs`
- Use: Development and testing
- No setup required

### 2. **Entity Framework Core** (SQL Databases)

- Location: `Shared.Infrastructure.EntityFramework/`
- Supports: SQL Server, PostgreSQL, MySQL, SQLite
- Features: Migrations, LINQ queries, transactions

### 3. **Cosmos DB** (Azure NoSQL)

- Location: `Shared.Infrastructure.CosmosDb/`
- Features: **Automatic partition key extraction**
- Partition keys defined via `[PartitionKey]` attribute

### 4. **MongoDB** (NoSQL)

- Location: `Shared.Infrastructure.MongoDb/`
- Features: **Automatic shard key extraction**
- Shard keys defined via `[ShardKey]` attribute

## Key Features

### Automatic Key Management

**Problem**: Cosmos DB requires partition keys, MongoDB uses shard keys - how to handle both?

**Solution**: Use attributes on your entities:

```csharp
[PartitionKey(nameof(TenantId), PartitionKeyPath = "/tenantId")]
[ShardKey(nameof(TenantId), Strategy = "hashed")]
[CollectionName("products")]
public class Product : EntityBase
{
    public required string TenantId { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
```

The repositories **automatically extract** these keys when performing operations:

- Cosmos DB: Uses partition key for all operations
- MongoDB: Uses shard key to optimize queries
- EF Core: Ignores these attributes
- InMemory: Ignores these attributes

### Complete Abstraction

Your application code doesn't change when you switch databases:

```csharp
// This works with ALL database implementations
public class CreateProductCommandHandler(IRepository<Product> repository)
{
    public async Task<Product> HandleAsync(CreateProductCommand command)
    {
        var product = new Product
        {
            TenantId = command.TenantId,
            Name = command.Name,
            Price = command.Price
        };
        return await repository.CreateAsync(product);
    }
}
```

## Project Structure

```
src/Shared/
├── YourCompany.YourProject.Shared.Core/
│   ├── Abstractions/
│   │   └── IRepository.cs                    # Interface all repos implement
│   ├── Attributes/
│   │   ├── PartitionKeyAttribute.cs          # For Cosmos DB
│   │   ├── ShardKeyAttribute.cs              # For MongoDB
│   │   └── CollectionNameAttribute.cs        # Custom collection names
│   └── Helpers/
│       ├── PartitionKeyHelper.cs             # Extracts partition keys
│       ├── ShardKeyHelper.cs                 # Extracts shard keys
│       └── CollectionNameHelper.cs           # Gets collection names
│
├── YourCompany.YourProject.Shared.Infrastructure/
│   └── Repositories/
│       └── InMemoryRepository.cs             # Default implementation
│
├── YourCompany.YourProject.Shared.Infrastructure.EntityFramework/
│   ├── Repositories/
│   │   └── EfRepository.cs                   # Generic EF Core repo
│   └── DependencyInjection.cs
│
├── YourCompany.YourProject.Shared.Infrastructure.CosmosDb/
│   ├── Repositories/
│   │   └── CosmosDbRepository.cs             # With partition key support
│   └── DependencyInjection.cs
│
└── YourCompany.YourProject.Shared.Infrastructure.MongoDb/
    ├── Repositories/
    │   └── MongoDbRepository.cs              # With shard key support
    └── DependencyInjection.cs
```

## Quick Start Examples

### Using In-Memory (Default)

```csharp
services.AddRepository<Product>();
```

### Using Entity Framework Core

```csharp
services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddEfRepository<Product, MyDbContext>();
```

### Using Cosmos DB

```csharp
services.AddCosmosDbClient(cosmosConnectionString);
services.AddCosmosDbRepository<Product>("MyDatabase");
```

### Using MongoDB

```csharp
services.AddMongoDbClient(mongoConnectionString);
services.AddMongoDbRepository<Product>("MyDatabase");
```

## Attribute Reference

### `[PartitionKey]` - For Cosmos DB

```csharp
// Single property
[PartitionKey(nameof(TenantId))]

// Composite key
[PartitionKey(nameof(TenantId), nameof(Region))]

// With explicit path
[PartitionKey(nameof(TenantId), PartitionKeyPath = "/tenantId")]
```

### `[ShardKey]` - For MongoDB

```csharp
// Single property with hashed strategy
[ShardKey(nameof(TenantId), Strategy = "hashed")]

// Compound key
[ShardKey(nameof(TenantId), nameof(Category))]

// Range-based strategy
[ShardKey(nameof(CreatedDate), Strategy = "range")]
```

### `[CollectionName]` - Custom Collection Name

```csharp
// Custom name instead of default pluralized name
[CollectionName("products")]
```

## Configuration-Based Switching

**appsettings.json**:

```json
{
  "Database": {
    "Provider": "CosmosDb" // "InMemory" | "SqlServer" | "CosmosDb" | "MongoDb"
  }
}
```

**DependencyInjection.cs**:

```csharp
var provider = configuration["Database:Provider"];
switch (provider)
{
    case "InMemory":
        services.AddRepository<Product>();
        break;
    case "SqlServer":
        services.AddEfRepository<Product, MyDbContext>();
        break;
    case "CosmosDb":
        services.AddCosmosDbRepository<Product>("MyDb");
        break;
    case "MongoDb":
        services.AddMongoDbRepository<Product>("MyDb");
        break;
}
```

## Documentation

- **Full Guide**: [DATABASE_IMPLEMENTATIONS.md](DATABASE_IMPLEMENTATIONS.md)
- **Architecture**: [ARCHITECTURE.md](ARCHITECTURE.md)
- **Module Creation**: [MODULECREATION.md](MODULECREATION.md)

## Benefits

✅ **Single Interface**: Use `IRepository<TEntity>` everywhere
✅ **Zero Code Changes**: Switch databases with configuration only
✅ **Automatic Key Management**: Partition/shard keys extracted automatically
✅ **Multi-Tenancy Ready**: TenantId-based partitioning/sharding
✅ **Type Safe**: Compile-time validation of partition/shard keys
✅ **Testable**: Use InMemory for unit tests, real DB for integration tests

## Example Entity

```csharp
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace YourCompany.YourProject.YourFeature.Domain.Entities;

/// <summary>
/// Product entity configured for multi-database support.
/// Works seamlessly with InMemory, EF Core, Cosmos DB, and MongoDB.
/// </summary>
[PartitionKey(nameof(TenantId), PartitionKeyPath = "/tenantId")]
[ShardKey(nameof(TenantId), Strategy = "hashed")]
[CollectionName("products")]
public class Product : EntityBase
{
    /// <summary>
    /// Tenant ID for multi-tenancy.
    /// Used as partition key (Cosmos) and shard key (MongoDB).
    /// </summary>
    public required string TenantId { get; set; }

    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
```

## Next Steps

1. **Choose Your Database**: See [DATABASE_IMPLEMENTATIONS.md](DATABASE_IMPLEMENTATIONS.md) for setup instructions
2. **Configure Your Entities**: Add partition/shard key attributes as needed
3. **Register Repositories**: Update `DependencyInjection.cs` in your Infrastructure project
4. **Test**: Verify with your chosen database

---

**Built with flexibility in mind - one interface, multiple implementations!** 🚀
