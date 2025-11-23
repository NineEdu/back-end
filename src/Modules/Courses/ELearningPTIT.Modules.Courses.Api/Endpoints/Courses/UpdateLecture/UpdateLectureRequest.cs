using ELearningPTIT.Modules.Courses.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.UpdateLecture;

public class UpdateLectureRequest
{
    public string CourseId { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public string LectureId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public LectureType Type { get; set; } = LectureType.Video;
    public int Order { get; set; }
    public int DurationMinutes { get; set; }
    public string? VideoUrl { get; set; }
    public string? ArticleContent { get; set; }
    public List<string> ResourceUrls { get; set; } = new();
    public bool IsPreviewable { get; set; }
}
