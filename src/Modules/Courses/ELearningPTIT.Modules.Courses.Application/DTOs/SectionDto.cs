namespace ELearningPTIT.Modules.Courses.Application.DTOs;

public class SectionDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public List<LectureDto> Lectures { get; set; } = new();
    public bool IsPublished { get; set; }
}
