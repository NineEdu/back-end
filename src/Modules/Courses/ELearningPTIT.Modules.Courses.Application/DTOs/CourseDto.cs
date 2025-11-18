using ELearningPTIT.Modules.Courses.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Courses.Application.DTOs;

public class CourseDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
    public string InstructorId { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? PreviewVideoUrl { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public CourseStatus Status { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public string Language { get; set; } = "en";
    public List<string> Subtitles { get; set; } = new();
    public List<string> LearningOutcomes { get; set; } = new();
    public List<string> Requirements { get; set; } = new();
    public List<string> TargetAudience { get; set; } = new();
    public List<SectionDto> Sections { get; set; } = new();
    public int TotalDurationMinutes { get; set; }
    public int TotalLectures { get; set; }
    public int TotalEnrollments { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
