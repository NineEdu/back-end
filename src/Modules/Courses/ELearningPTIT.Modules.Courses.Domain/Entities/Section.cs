using MongoDB.Bson.Serialization.Attributes;

namespace ELearningPTIT.Modules.Courses.Domain.Entities;

/// <summary>
/// Represents a section within a course curriculum
/// </summary>
public class Section
{
    [BsonElement("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("order")]
    public int Order { get; set; } = 0;

    [BsonElement("lectures")]
    public List<Lecture> Lectures { get; set; } = new();

    [BsonElement("isPublished")]
    public bool IsPublished { get; set; } = false;
}
