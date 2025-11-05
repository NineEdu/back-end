namespace YourCompany.YourProject.Shared.Core.Attributes;

/// <summary>
/// Specifies the property or properties to use as the shard key for MongoDB.
/// Multiple properties can be specified for compound shard keys.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ShardKeyAttribute : Attribute
{
    /// <summary>
    /// Gets the property names that make up the shard key.
    /// </summary>
    public string[] PropertyNames { get; }

    /// <summary>
    /// Gets or sets the shard key strategy (e.g., "hashed" or "range").
    /// Default is "hashed".
    /// </summary>
    public string Strategy { get; set; } = "hashed";

    /// <summary>
    /// Initializes a new instance with a single shard key property.
    /// </summary>
    /// <param name="propertyName">The name of the property to use as shard key</param>
    public ShardKeyAttribute(string propertyName)
    {
        PropertyNames = new[] { propertyName };
    }

    /// <summary>
    /// Initializes a new instance with multiple shard key properties (compound key).
    /// </summary>
    /// <param name="propertyNames">The names of the properties to use as shard key</param>
    public ShardKeyAttribute(params string[] propertyNames)
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
