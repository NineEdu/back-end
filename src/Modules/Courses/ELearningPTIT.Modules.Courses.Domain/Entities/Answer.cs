using MongoDB.Bson.Serialization.Attributes;

namespace ELearningPTIT.Modules.Courses.Domain.Entities;

/// <summary>
/// Represents an answer to a Q&A question
/// </summary>
public class Answer
{
    [BsonElement("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [BsonElement("userId")]
    [BsonRequired]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("content")]
    [BsonRequired]
    public string Content { get; set; } = string.Empty;

    [BsonElement("isInstructorAnswer")]
    public bool IsInstructorAnswer { get; set; } = false;

    [BsonElement("isAcceptedAnswer")]
    public bool IsAcceptedAnswer { get; set; } = false;

    [BsonElement("upvoteCount")]
    public int UpvoteCount { get; set; } = 0;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("isFlagged")]
    public bool IsFlagged { get; set; } = false;
}
