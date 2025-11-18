namespace ELearningPTIT.Modules.Courses.Application.DTOs;

public class ReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string CourseId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public bool IsPublished { get; set; }
    public int HelpfulCount { get; set; }
    public string? InstructorResponse { get; set; }
    public DateTime? InstructorResponseAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
