namespace YourCompany.YourProject.Shared.Api.Abstractions;

/// <summary>
/// Base class for entities that have a unique identifier and timestamps for creation and update.
/// </summary>
public class EntityBaseDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the entity was created.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the entity was last updated.
    /// </summary>
    public required DateTime UpdatedAt { get; set; }
}
