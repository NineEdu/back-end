using MongoDB.Bson.Serialization.Attributes;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Courses.Domain.Entities;

/// <summary>
/// Represents a lecture within a section
/// </summary>
public class Lecture
{
    [BsonElement("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("type")]
    public LectureType Type { get; set; } = LectureType.Video;

    [BsonElement("order")]
    public int Order { get; set; } = 0;

    [BsonElement("durationMinutes")]
    public int DurationMinutes { get; set; } = 0;

    [BsonElement("videoUrl")]
    public string? VideoUrl { get; set; }

    [BsonElement("articleContent")]
    public string? ArticleContent { get; set; }

    [BsonElement("resourceUrls")]
    public List<string> ResourceUrls { get; set; } = new();

    [BsonElement("isPreviewable")]
    public bool IsPreviewable { get; set; } = false;

    [BsonElement("isPublished")]
    public bool IsPublished { get; set; } = false;
}
