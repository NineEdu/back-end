namespace YourCompany.YourProject.Shared.Core.Attributes;

/// <summary>
/// Specifies the collection/container name for the entity in document databases.
/// If not specified, the entity class name will be used (pluralized).
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class CollectionNameAttribute : Attribute
{
    /// <summary>
    /// Gets the collection/container name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance with the specified collection name.
    /// </summary>
    /// <param name="name">The name of the collection/container</param>
    public CollectionNameAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Collection name cannot be null or empty", nameof(name));
        }
        Name = name;
    }
}
