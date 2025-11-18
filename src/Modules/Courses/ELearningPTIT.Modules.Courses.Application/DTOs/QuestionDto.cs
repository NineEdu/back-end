namespace ELearningPTIT.Modules.Courses.Application.DTOs;

public class QuestionDto
{
    public string Id { get; set; } = string.Empty;
    public string CourseId { get; set; } = string.Empty;
    public string? LectureId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<AnswerDto> Answers { get; set; } = new();
    public int UpvoteCount { get; set; }
    public bool IsResolved { get; set; }
    public DateTime CreatedAt { get; set; }
}
