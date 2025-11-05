using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace YourCompany.YourProject.YourFeature.Domain.Entities;

/// <summary>
/// Product entity with partition key and shard key configured for multi-database support.
/// - TenantId is used as the partition key for Cosmos DB
/// - TenantId is used as the shard key for MongoDB
/// - Collection name is explicitly set to "products"
/// </summary>
[PartitionKey(nameof(TenantId), PartitionKeyPath = "/tenantId")]
[ShardKey(nameof(TenantId), Strategy = "hashed")]
[CollectionName("products")]
public class Product : EntityBase
{
    /// <summary>
    /// Tenant identifier for multi-tenancy support.
    /// Used as partition/shard key for optimal database performance.
    /// </summary>
    public required string TenantId { get; set; }

    public required string Name { get; set; }
    public required decimal Price { get; set; }
}
