namespace YourCompany.YourProject.Shared.Core.Attributes;

/// <summary>
/// Specifies the property or properties to use as the partition key for Cosmos DB.
/// Multiple properties can be specified for composite partition keys.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PartitionKeyAttribute : Attribute
{
    /// <summary>
    /// Gets the property names that make up the partition key.
    /// </summary>
    public string[] PropertyNames { get; }

    /// <summary>
    /// Gets the partition key path for Cosmos DB (e.g., "/tenantId").
    /// If not specified, it will be derived from PropertyNames.
    /// </summary>
    public string? PartitionKeyPath { get; set; }

    /// <summary>
    /// Initializes a new instance with a single partition key property.
    /// </summary>
    /// <param name="propertyName">The name of the property to use as partition key</param>
    public PartitionKeyAttribute(string propertyName)
    {
        PropertyNames = new[] { propertyName };
    }

    /// <summary>
    /// Initializes a new instance with multiple partition key properties (composite key).
    /// </summary>
    /// <param name="propertyNames">The names of the properties to use as partition key</param>
    public PartitionKeyAttribute(params string[] propertyNames)
    {
        if (propertyNames == null || propertyNames.Length == 0)
        {
            throw new ArgumentException(
                "At least one property name must be specified",
                nameof(propertyNames)
            );
        }
        PropertyNames = propertyNames;
    }
}
