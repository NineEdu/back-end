using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;

namespace ELearningPTIT.Modules.Courses.Domain.Entities;

/// <summary>
/// Represents a student review/rating for a course
/// </summary>
[CollectionName("reviews")]
public class Review : EntityBase
{
    public Review()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonElement("courseId")]
    [BsonRequired]
    public string CourseId { get; set; } = string.Empty;

    [BsonElement("userId")]
    [BsonRequired]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("rating")]
    [BsonRequired]
    public int Rating { get; set; } = 5; // 1-5 stars

    [BsonElement("comment")]
    public string? Comment { get; set; }

    [BsonElement("isPublished")]
    public bool IsPublished { get; set; } = true;

    [BsonElement("isFlagged")]
    public bool IsFlagged { get; set; } = false;

    [BsonElement("helpfulCount")]
    public int HelpfulCount { get; set; } = 0;

    [BsonElement("reportCount")]
    public int ReportCount { get; set; } = 0;

    [BsonElement("instructorResponse")]
    public string? InstructorResponse { get; set; }

    [BsonElement("instructorResponseAt")]
    public DateTime? InstructorResponseAt { get; set; }
}
