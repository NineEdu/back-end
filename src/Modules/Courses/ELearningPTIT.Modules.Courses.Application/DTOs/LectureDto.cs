using ELearningPTIT.Modules.Courses.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Courses.Application.DTOs;

public class LectureDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public LectureType Type { get; set; }
    public int Order { get; set; }
    public int DurationMinutes { get; set; }
    public string? VideoUrl { get; set; }
    public string? ArticleContent { get; set; }
    public List<string> ResourceUrls { get; set; } = new();
    public bool IsPreviewable { get; set; }
    public bool IsPublished { get; set; }
}
