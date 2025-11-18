using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Courses.Domain.Entities;

/// <summary>
/// Represents a course in the learning platform
/// </summary>
[CollectionName("courses")]
public class Course : EntityBase
{
    public Course()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonElement("title")]
    [BsonRequired]
    public string Title { get; set; } = string.Empty;

    [BsonElement("slug")]
    [BsonRequired]
    public string Slug { get; set; } = string.Empty;

    [BsonElement("subtitle")]
    public string? Subtitle { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("instructorId")]
    [BsonRequired]
    public string InstructorId { get; set; } = string.Empty;

    [BsonElement("categoryId")]
    [BsonRequired]
    public string CategoryId { get; set; } = string.Empty;

    [BsonElement("thumbnailUrl")]
    public string? ThumbnailUrl { get; set; }

    [BsonElement("previewVideoUrl")]
    public string? PreviewVideoUrl { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; } = 0;

    [BsonElement("discountPrice")]
    public decimal? DiscountPrice { get; set; }

    [BsonElement("status")]
    public CourseStatus Status { get; set; } = CourseStatus.Draft;

    [BsonElement("difficultyLevel")]
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;

    [BsonElement("language")]
    public string Language { get; set; } = "en";

    [BsonElement("subtitles")]
    public List<string> Subtitles { get; set; } = new();

    [BsonElement("learningOutcomes")]
    public List<string> LearningOutcomes { get; set; } = new();

    [BsonElement("requirements")]
    public List<string> Requirements { get; set; } = new();

    [BsonElement("targetAudience")]
    public List<string> TargetAudience { get; set; } = new();

    [BsonElement("sections")]
    public List<Section> Sections { get; set; } = new();

    [BsonElement("totalDurationMinutes")]
    public int TotalDurationMinutes { get; set; } = 0;

    [BsonElement("totalLectures")]
    public int TotalLectures { get; set; } = 0;

    [BsonElement("totalEnrollments")]
    public int TotalEnrollments { get; set; } = 0;

    [BsonElement("averageRating")]
    public double AverageRating { get; set; } = 0.0;

    [BsonElement("totalReviews")]
    public int TotalReviews { get; set; } = 0;

    [BsonElement("publishedAt")]
    public DateTime? PublishedAt { get; set; }

    [BsonElement("lastUpdatedAt")]
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    // Approval workflow fields
    [BsonElement("submittedForApprovalAt")]
    public DateTime? SubmittedForApprovalAt { get; set; }

    [BsonElement("approvedBy")]
    public string? ApprovedBy { get; set; }

    [BsonElement("approvedAt")]
    public DateTime? ApprovedAt { get; set; }

    [BsonElement("rejectedBy")]
    public string? RejectedBy { get; set; }

    [BsonElement("rejectedAt")]
    public DateTime? RejectedAt { get; set; }

    [BsonElement("rejectionReason")]
    public string? RejectionReason { get; set; }

    // Helper methods
    public bool IsPublished() => Status == CourseStatus.Published;

    public bool CanBePublished() => Sections.Any() && Sections.All(s => s.Lectures.Any());

    public void CalculateTotals()
    {
        TotalLectures = Sections.Sum(s => s.Lectures.Count);
        TotalDurationMinutes = Sections.Sum(s => s.Lectures.Sum(l => l.DurationMinutes));
    }
}
