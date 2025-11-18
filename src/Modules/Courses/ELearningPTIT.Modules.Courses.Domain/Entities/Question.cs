using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace ELearningPTIT.Modules.Courses.Domain.Entities;

/// <summary>
/// Represents a Q&A question asked by a student in a course
/// </summary>
[CollectionName("questions")]
public class Question : EntityBase
{
    public Question()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonElement("courseId")]
    [BsonRequired]
    public string CourseId { get; set; } = string.Empty;

    [BsonElement("lectureId")]
    public string? LectureId { get; set; }

    [BsonElement("userId")]
    [BsonRequired]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; } = string.Empty;

    [BsonElement("content")]
    [BsonRequired]
    public string Content { get; set; } = string.Empty;

    [BsonElement("answers")]
    public List<Answer> Answers { get; set; } = new();

    [BsonElement("upvoteCount")]
    public int UpvoteCount { get; set; } = 0;

    [BsonElement("isResolved")]
    public bool IsResolved { get; set; } = false;

    [BsonElement("isFlagged")]
    public bool IsFlagged { get; set; } = false;
}
