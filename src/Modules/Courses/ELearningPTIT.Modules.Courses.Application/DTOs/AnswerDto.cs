namespace ELearningPTIT.Modules.Courses.Application.DTOs;

public class AnswerDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsInstructorAnswer { get; set; }
    public bool IsAcceptedAnswer { get; set; }
    public int UpvoteCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
