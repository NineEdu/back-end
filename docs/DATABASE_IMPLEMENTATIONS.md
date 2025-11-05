# Database Implementations

This guide explains how to use the different database implementations available in this boilerplate: In-Memory, Entity Framework Core (SQL), Cosmos DB (NoSQL), and MongoDB (NoSQL).

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Partition Keys & Shard Keys](#partition-keys--shard-keys)
- [Implementation Guides](#implementation-guides)
  - [In-Memory (Default)](#in-memory-default)
  - [Entity Framework Core (SQL)](#entity-framework-core-sql)
  - [Cosmos DB](#cosmos-db)
  - [MongoDB](#mongodb)
- [Entity Configuration](#entity-configuration)
- [Switching Between Databases](#switching-between-databases)

## Overview

The boilerplate provides four repository implementations, all implementing the same `IRepository<TEntity>` interface:

| Implementation         | Use Case             | Features                            |
| ---------------------- | -------------------- | ----------------------------------- |
| **InMemoryRepository** | Development, Testing | Fast, no setup required             |
| **EfRepository**       | SQL Databases        | EF Core, migrations, LINQ support   |
| **CosmosDbRepository** | Azure Cosmos DB      | Partition keys, global distribution |
| **MongoDbRepository**  | MongoDB              | Shard keys, flexible schema         |

All implementations are **completely interchangeable** thanks to the repository pattern.

## Architecture

```
┌─────────────────────────────────────────────────┐
│           Application Layer                      │
│        (Uses IRepository<TEntity>)               │
└─────────────────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────┐
│        Shared.Core.Abstractions                  │
│           IRepository<TEntity>                   │
└─────────────────────────────────────────────────┘
                      │
        ┌─────────────┼─────────────┬──────────────┐
        ▼             ▼             ▼              ▼
┌─────────────┐ ┌──────────┐ ┌────────────┐ ┌──────────┐
│  InMemory   │ │ EF Core  │ │  Cosmos DB │ │ MongoDB  │
│ Repository  │ │Repository│ │ Repository │ │Repository│
└─────────────┘ └──────────┘ └────────────┘ └──────────┘
```

## Partition Keys & Shard Keys

### Why Are They Important?

- **Cosmos DB Partition Keys**: Required for data distribution and performance
- **MongoDB Shard Keys**: Optimizes queries in sharded clusters

### Defining Keys with Attributes

Use attributes on your entity classes:

```csharp
using YourCompany.YourProject.Shared.Core.Attributes;

[PartitionKey(nameof(TenantId), PartitionKeyPath = "/tenantId")]  // For Cosmos DB
[ShardKey(nameof(TenantId), Strategy = "hashed")]                 // For MongoDB
[CollectionName("products")]                                       // Custom collection name
public class Product : EntityBase
{
    public required string TenantId { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
```

### Available Attributes

#### `[PartitionKey]`

- **Purpose**: Specifies partition key for Cosmos DB
- **Parameters**:
  - `propertyName`: Property to use as partition key
  - `PartitionKeyPath`: Optional explicit path (e.g., `/tenantId`)

```csharp
// Single partition key
[PartitionKey(nameof(TenantId))]

// Composite partition key
[PartitionKey(nameof(TenantId), nameof(Region))]

// With explicit path
[PartitionKey(nameof(TenantId), PartitionKeyPath = "/tenantId")]
```

#### `[ShardKey]`

- **Purpose**: Specifies shard key for MongoDB
- **Parameters**:
  - `propertyName`: Property to use as shard key
  - `Strategy`: "hashed" or "range" (default: "hashed")

```csharp
// Single shard key with hashed strategy
[ShardKey(nameof(TenantId), Strategy = "hashed")]

// Compound shard key
[ShardKey(nameof(TenantId), nameof(Category))]

// Range-based sharding
[ShardKey(nameof(CreatedDate), Strategy = "range")]
```

#### `[CollectionName]`

- **Purpose**: Specifies custom collection/container name
- **Default**: Entity name pluralized and lowercased

```csharp
[CollectionName("products")]  // Uses "products" instead of default
```

## Implementation Guides

### In-Memory (Default)

The simplest implementation, useful for development and testing.

**Setup in Infrastructure/DependencyInjection.cs:**

```csharp
using YourCompany.YourProject.Shared.Infrastructure;

public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
{
    services.AddRepository<Product>();
    return services;
}
```

**Advantages:**

- ✅ No configuration needed
- ✅ Fast for development
- ✅ Works immediately

**Limitations:**

- ❌ Data lost on restart
- ❌ Not suitable for production

---

### Entity Framework Core (SQL)

For SQL databases like SQL Server, PostgreSQL, MySQL, SQLite.

#### 1. Install Packages

```bash
cd src/Modules/YourFeature/YourCompany.YourProject.Modules.YourFeature.Infrastructure

# Choose your database provider:
dotnet add package Microsoft.EntityFrameworkCore.SqlServer      # SQL Server
# OR
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL        # PostgreSQL
# OR
dotnet add package Pomelo.EntityFrameworkCore.MySql             # MySQL
# OR
dotnet add package Microsoft.EntityFrameworkCore.Sqlite         # SQLite

# Add reference to Shared.Infrastructure.EntityFramework
dotnet add reference ../../../Shared/YourCompany.YourProject.Shared.Infrastructure.EntityFramework/YourCompany.YourProject.Shared.Infrastructure.EntityFramework.csproj
```

#### 2. Create DbContext

Create `YourFeatureDbContext.cs` in Infrastructure:

```csharp
using Microsoft.EntityFrameworkCore;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.YourFeature.Infrastructure;

public class YourFeatureDbContext : DbContext
{
    public YourFeatureDbContext(DbContextOptions<YourFeatureDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entities
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.HasIndex(e => e.TenantId); // Index for partition key
        });
    }
}
```

#### 3. Update DependencyInjection.cs

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Shared.Infrastructure.EntityFramework;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.YourFeature.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<YourFeatureDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("YourFeatureDb"),
                b => b.MigrationsAssembly(typeof(YourFeatureDbContext).Assembly.FullName)
            )
        );

        // Register EF Core repository
        services.AddEfRepository<Product, YourFeatureDbContext>();

        return services;
    }
}
```

#### 4. Add Connection String

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "YourFeatureDb": "Server=localhost;Database=YourFeatureDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

#### 5. Create and Apply Migrations

```bash
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Navigate to Infrastructure project
cd src/Modules/YourFeature/YourCompany.YourProject.Modules.YourFeature.Infrastructure

# Create initial migration
dotnet ef migrations add InitialCreate --startup-project ../../../Hosts/YourCompany.YourProject.Hosts.Main

# Apply migration
dotnet ef database update --startup-project ../../../Hosts/YourCompany.YourProject.Hosts.Main
```

---

### Cosmos DB

For Azure Cosmos DB with automatic partition key extraction.

#### 1. Install Packages

```bash
cd src/Modules/YourFeature/YourCompany.YourProject.Modules.YourFeature.Infrastructure

dotnet add reference ../../../Shared/YourCompany.YourProject.Shared.Infrastructure.CosmosDb/YourCompany.YourProject.Shared.Infrastructure.CosmosDb.csproj
```

#### 2. Update DependencyInjection.cs

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Shared.Infrastructure.CosmosDb;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.YourFeature.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register Cosmos DB client
        var cosmosConnectionString = configuration.GetConnectionString("CosmosDb");
        services.AddCosmosDbClient(cosmosConnectionString);

        // Register Cosmos DB repository
        var databaseName = configuration["CosmosDb:DatabaseName"] ?? "YourFeatureDb";
        services.AddCosmosDbRepository<Product>(databaseName);

        return services;
    }
}
```

#### 3. Configure Cosmos DB Settings

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "CosmosDb": "AccountEndpoint=https://your-account.documents.azure.com:443/;AccountKey=your-key;"
  },
  "CosmosDb": {
    "DatabaseName": "YourFeatureDb"
  }
}
```

#### 4. Create Container

The container needs to be created with the correct partition key path. You can do this via:

**Option A: Azure Portal**

- Create container named `products`
- Set partition key to `/tenantId`

**Option B: Programmatically**

```csharp
using Microsoft.Azure.Cosmos;

// In your startup or initialization code
var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();
var database = await cosmosClient.CreateDatabaseIfNotExistsAsync("YourFeatureDb");

await database.Database.CreateContainerIfNotExistsAsync(
    new ContainerProperties
    {
        Id = "products",
        PartitionKeyPath = "/tenantId"
    },
    throughput: 400
);
```

#### 5. Partition Key Extraction

The repository **automatically extracts** the partition key using the `[PartitionKey]` attribute:

```csharp
[PartitionKey(nameof(TenantId), PartitionKeyPath = "/tenantId")]
public class Product : EntityBase
{
    public required string TenantId { get; set; }
    // ...
}
```

---

### MongoDB

For MongoDB with automatic shard key extraction.

#### 1. Install Packages

```bash
cd src/Modules/YourFeature/YourCompany.YourProject.Modules.YourFeature.Infrastructure

dotnet add reference ../../../Shared/YourCompany.YourProject.Shared.Infrastructure.MongoDb/YourCompany.YourProject.Shared.Infrastructure.MongoDb.csproj
```

#### 2. Update DependencyInjection.cs

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourCompany.YourProject.Shared.Infrastructure.MongoDb;
using YourCompany.YourProject.YourFeature.Domain.Entities;

namespace YourCompany.YourProject.YourFeature.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register MongoDB client
        var mongoConnectionString = configuration.GetConnectionString("MongoDb");
        services.AddMongoDbClient(mongoConnectionString);

        // Register MongoDB repository
        var databaseName = configuration["MongoDb:DatabaseName"] ?? "YourFeatureDb";
        services.AddMongoDbRepository<Product>(databaseName);

        return services;
    }
}
```

#### 3. Configure MongoDB Settings

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27017"
  },
  "MongoDb": {
    "DatabaseName": "YourFeatureDb"
  }
}
```

#### 4. Configure Sharding (Optional)

For production MongoDB clusters with sharding:

```javascript
// Connect to MongoDB shell
mongosh;

// Enable sharding on database
sh.enableSharding("YourFeatureDb");

// Create shard key on collection
sh.shardCollection("YourFeatureDb.products", { tenantId: "hashed" });
```

#### 5. Shard Key Extraction

The repository **automatically uses** the shard key for optimized queries:

```csharp
[ShardKey(nameof(TenantId), Strategy = "hashed")]
public class Product : EntityBase
{
    public required string TenantId { get; set; }
    // ...
}
```

---

## Entity Configuration

### Basic Entity

```csharp
using YourCompany.YourProject.Shared.Core.Abstractions;

public class Product : EntityBase
{
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
```

### Multi-Database Entity with Partition/Shard Keys

```csharp
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

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

### Advanced: Composite Keys

```csharp
[PartitionKey(nameof(TenantId), nameof(Region))]
[ShardKey(nameof(TenantId), nameof(Region))]
public class MultiRegionProduct : EntityBase
{
    public required string TenantId { get; set; }
    public required string Region { get; set; }
    public required string Name { get; set; }
}
```

---

## Switching Between Databases

### Using Configuration

**appsettings.Development.json** (InMemory):

```json
{
  "Database": {
    "Provider": "InMemory"
  }
}
```

**appsettings.Production.json** (Cosmos DB):

```json
{
  "Database": {
    "Provider": "CosmosDb"
  },
  "ConnectionStrings": {
    "CosmosDb": "AccountEndpoint=https://..."
  }
}
```

### Conditional Registration

Update `DependencyInjection.cs`:

```csharp
public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    var databaseProvider = configuration["Database:Provider"] ?? "InMemory";

    switch (databaseProvider)
    {
        case "InMemory":
            services.AddRepository<Product>();
            break;

        case "SqlServer":
            services.AddDbContext<YourFeatureDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.AddEfRepository<Product, YourFeatureDbContext>();
            break;

        case "CosmosDb":
            services.AddCosmosDbClient(configuration.GetConnectionString("CosmosDb"));
            services.AddCosmosDbRepository<Product>(
                configuration["CosmosDb:DatabaseName"] ?? "YourFeatureDb");
            break;

        case "MongoDb":
            services.AddMongoDbClient(configuration.GetConnectionString("MongoDb"));
            services.AddMongoDbRepository<Product>(
                configuration["MongoDb:DatabaseName"] ?? "YourFeatureDb");
            break;

        default:
            throw new InvalidOperationException($"Unknown database provider: {databaseProvider}");
    }

    return services;
}
```

---

## Best Practices

### 1. Choose the Right Partition/Shard Key

✅ **Good partition keys:**

- High cardinality (many unique values)
- Even distribution of data
- Frequently used in queries
- Examples: `TenantId`, `UserId`, `CustomerId`

❌ **Bad partition keys:**

- Low cardinality (few unique values)
- Uneven distribution
- Examples: `Status`, `Category`, `Country`

### 2. Always Include Partition Key in Queries

```csharp
// Good: Includes partition key
var products = await repository.QueryAsync(
    p => p.TenantId == tenantId && p.Price > 100);

// Less efficient: Missing partition key
var products = await repository.QueryAsync(p => p.Price > 100);
```

### 3. Use Configuration-Based Switching

Don't hardcode database selection. Use configuration to switch between databases for different environments.

### 4. Testing

Use InMemory for unit tests, real database for integration tests:

```csharp
// Unit test
services.AddRepository<Product>(); // InMemory

// Integration test
services.AddEfRepository<Product, TestDbContext>(); // Real DB
```

---

## Summary

| Feature              | InMemory | EF Core       | Cosmos DB      | MongoDB         |
| -------------------- | -------- | ------------- | -------------- | --------------- |
| Setup Complexity     | ⭐ Easy  | ⭐⭐ Moderate | ⭐⭐⭐ Complex | ⭐⭐⭐ Complex  |
| Performance          | Fast     | Fast-Moderate | Fast           | Fast            |
| Scalability          | Low      | Moderate      | High           | High            |
| Production Ready     | ❌ No    | ✅ Yes        | ✅ Yes         | ✅ Yes          |
| Partition/Shard Keys | N/A      | N/A           | Required       | Optional        |
| Best For             | Dev/Test | SQL Apps      | Global Apps    | Flexible Schema |

---

**Next Steps:**

- See [ARCHITECTURE.md](ARCHITECTURE.md) for overall architecture
- See [MODULE_CREATION.md](MODULECREATION.md) for creating new modules
- See [FASTENDPOINTS.md](FASTENDPOINTS.md) for API implementation
