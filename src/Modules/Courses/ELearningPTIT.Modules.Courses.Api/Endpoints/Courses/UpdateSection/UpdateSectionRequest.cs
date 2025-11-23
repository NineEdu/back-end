namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.UpdateSection;

public class UpdateSectionRequest
{
    public string CourseId { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
}
