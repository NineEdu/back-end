using System.Reflection;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace YourCompany.YourProject.Shared.Core.Helpers;

/// <summary>
/// Helper class for extracting partition key values from entities.
/// </summary>
public static class PartitionKeyHelper
{
    /// <summary>
    /// Extracts the partition key value from an entity using the PartitionKeyAttribute.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <param name="entity">The entity instance</param>
    /// <returns>The partition key value as a string</returns>
    /// <exception cref="InvalidOperationException">Thrown when partition key attribute is not found or property is missing</exception>
    public static string GetPartitionKeyValue<TEntity>(TEntity entity)
        where TEntity : IEntityBase
    {
        var attribute = typeof(TEntity).GetCustomAttribute<PartitionKeyAttribute>();
        if (attribute == null)
        {
            throw new InvalidOperationException(
                $"Entity type {typeof(TEntity).Name} does not have a PartitionKeyAttribute defined."
            );
        }

        return GetPartitionKeyValue(entity, attribute);
    }

    /// <summary>
    /// Extracts the partition key value from an entity using the provided attribute.
    /// </summary>
    public static string GetPartitionKeyValue<TEntity>(
        TEntity entity,
        PartitionKeyAttribute attribute
    )
        where TEntity : IEntityBase
    {
        if (attribute.PropertyNames.Length == 1)
        {
            // Single partition key
            var propertyName = attribute.PropertyNames[0];
            var property = typeof(TEntity).GetProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Property {propertyName} not found on entity type {typeof(TEntity).Name}"
                );
            }

            var value = property.GetValue(entity);
            return value?.ToString()
                ?? throw new InvalidOperationException(
                    $"Partition key property {propertyName} has null value"
                );
        }
        else
        {
            // Composite partition key - concatenate values
            var values = new List<string>();
            foreach (var propertyName in attribute.PropertyNames)
            {
                var property = typeof(TEntity).GetProperty(propertyName);
                if (property == null)
                {
                    throw new InvalidOperationException(
                        $"Property {propertyName} not found on entity type {typeof(TEntity).Name}"
                    );
                }

                var value = property.GetValue(entity);
                values.Add(
                    value?.ToString()
                        ?? throw new InvalidOperationException(
                            $"Partition key property {propertyName} has null value"
                        )
                );
            }

            return string.Join("_", values);
        }
    }

    /// <summary>
    /// Gets the partition key path for Cosmos DB from the attribute.
    /// </summary>
    public static string GetPartitionKeyPath<TEntity>()
        where TEntity : IEntityBase
    {
        var attribute = typeof(TEntity).GetCustomAttribute<PartitionKeyAttribute>();
        if (attribute == null)
        {
            throw new InvalidOperationException(
                $"Entity type {typeof(TEntity).Name} does not have a PartitionKeyAttribute defined."
            );
        }

        // If path is explicitly specified, use it
        if (!string.IsNullOrWhiteSpace(attribute.PartitionKeyPath))
        {
            return attribute.PartitionKeyPath;
        }

        // Otherwise, derive from property name (convert to camelCase and add /)
        var propertyName = attribute.PropertyNames[0];
        var camelCase = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        return $"/{camelCase}";
    }

    /// <summary>
    /// Checks if an entity has a partition key defined.
    /// </summary>
    public static bool HasPartitionKey<TEntity>()
        where TEntity : IEntityBase
    {
        return typeof(TEntity).GetCustomAttribute<PartitionKeyAttribute>() != null;
    }

    /// <summary>
    /// Tries to get the partition key value from an entity.
    /// </summary>
    public static bool TryGetPartitionKeyValue<TEntity>(TEntity entity, out string partitionKey)
        where TEntity : IEntityBase
    {
        try
        {
            partitionKey = GetPartitionKeyValue(entity);
            return true;
        }
        catch
        {
            partitionKey = string.Empty;
            return false;
        }
    }
}
