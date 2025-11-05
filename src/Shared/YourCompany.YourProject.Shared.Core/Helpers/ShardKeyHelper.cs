using System.Reflection;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace YourCompany.YourProject.Shared.Core.Helpers;

/// <summary>
/// Helper class for extracting shard key values from entities.
/// </summary>
public static class ShardKeyHelper
{
    /// <summary>
    /// Extracts the shard key value from an entity using the ShardKeyAttribute.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <param name="entity">The entity instance</param>
    /// <returns>A dictionary of property names and their values for the shard key</returns>
    /// <exception cref="InvalidOperationException">Thrown when shard key attribute is not found or property is missing</exception>
    public static Dictionary<string, object> GetShardKeyValue<TEntity>(TEntity entity)
        where TEntity : IEntityBase
    {
        var attribute = typeof(TEntity).GetCustomAttribute<ShardKeyAttribute>();
        if (attribute == null)
        {
            throw new InvalidOperationException(
                $"Entity type {typeof(TEntity).Name} does not have a ShardKeyAttribute defined."
            );
        }

        return GetShardKeyValue(entity, attribute);
    }

    /// <summary>
    /// Extracts the shard key value from an entity using the provided attribute.
    /// </summary>
    public static Dictionary<string, object> GetShardKeyValue<TEntity>(
        TEntity entity,
        ShardKeyAttribute attribute
    )
        where TEntity : IEntityBase
    {
        var shardKey = new Dictionary<string, object>();

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
            if (value == null)
            {
                throw new InvalidOperationException(
                    $"Shard key property {propertyName} has null value"
                );
            }

            shardKey[propertyName] = value;
        }

        return shardKey;
    }

    /// <summary>
    /// Gets the shard key property names from the attribute.
    /// </summary>
    public static string[] GetShardKeyPropertyNames<TEntity>()
        where TEntity : IEntityBase
    {
        var attribute = typeof(TEntity).GetCustomAttribute<ShardKeyAttribute>();
        if (attribute == null)
        {
            throw new InvalidOperationException(
                $"Entity type {typeof(TEntity).Name} does not have a ShardKeyAttribute defined."
            );
        }

        return attribute.PropertyNames;
    }

    /// <summary>
    /// Gets the shard key strategy (hashed or range) from the attribute.
    /// </summary>
    public static string GetShardKeyStrategy<TEntity>()
        where TEntity : IEntityBase
    {
        var attribute = typeof(TEntity).GetCustomAttribute<ShardKeyAttribute>();
        if (attribute == null)
        {
            throw new InvalidOperationException(
                $"Entity type {typeof(TEntity).Name} does not have a ShardKeyAttribute defined."
            );
        }

        return attribute.Strategy;
    }

    /// <summary>
    /// Checks if an entity has a shard key defined.
    /// </summary>
    public static bool HasShardKey<TEntity>()
        where TEntity : IEntityBase
    {
        return typeof(TEntity).GetCustomAttribute<ShardKeyAttribute>() != null;
    }

    /// <summary>
    /// Tries to get the shard key value from an entity.
    /// </summary>
    public static bool TryGetShardKeyValue<TEntity>(
        TEntity entity,
        out Dictionary<string, object> shardKey
    )
        where TEntity : IEntityBase
    {
        try
        {
            shardKey = GetShardKeyValue(entity);
            return true;
        }
        catch
        {
            shardKey = new Dictionary<string, object>();
            return false;
        }
    }
}
