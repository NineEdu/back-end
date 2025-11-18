using ELearningPTIT.Modules.Courses.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.CreateCourse;

public class CreateCourseRequest
{
    public required string Title { get; set; }
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
    public required string CategoryId { get; set; }
    public decimal Price { get; set; } = 0;
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
    public string Language { get; set; } = "en";
}
