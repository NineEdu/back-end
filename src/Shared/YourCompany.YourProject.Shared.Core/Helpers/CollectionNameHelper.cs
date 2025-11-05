using System.Reflection;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace YourCompany.YourProject.Shared.Core.Helpers;

/// <summary>
/// Helper class for extracting collection/container names from entities.
/// </summary>
public static class CollectionNameHelper
{
    /// <summary>
    /// Gets the collection/container name for an entity.
    /// If CollectionNameAttribute is not present, returns the pluralized entity name in lowercase.
    /// </summary>
    public static string GetCollectionName<TEntity>()
        where TEntity : IEntityBase
    {
        var attribute = typeof(TEntity).GetCustomAttribute<CollectionNameAttribute>();
        if (attribute != null)
        {
            return attribute.Name;
        }

        // Default: pluralize entity name and convert to lowercase
        var entityName = typeof(TEntity).Name;
        return Pluralize(entityName).ToLowerInvariant();
    }

    /// <summary>
    /// Simple pluralization logic. For more complex scenarios, consider using a library like Humanizer.
    /// </summary>
    private static string Pluralize(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return word;
        }

        // Simple English pluralization rules
        if (
            word.EndsWith("y", StringComparison.OrdinalIgnoreCase)
            && word.Length > 1
            && !IsVowel(word[word.Length - 2])
        )
        {
            return word.Substring(0, word.Length - 1) + "ies";
        }

        if (
            word.EndsWith("s", StringComparison.OrdinalIgnoreCase)
            || word.EndsWith("x", StringComparison.OrdinalIgnoreCase)
            || word.EndsWith("z", StringComparison.OrdinalIgnoreCase)
            || word.EndsWith("ch", StringComparison.OrdinalIgnoreCase)
            || word.EndsWith("sh", StringComparison.OrdinalIgnoreCase)
        )
        {
            return word + "es";
        }

        return word + "s";
    }

    private static bool IsVowel(char c)
    {
        return "aeiouAEIOU".Contains(c);
    }
}
