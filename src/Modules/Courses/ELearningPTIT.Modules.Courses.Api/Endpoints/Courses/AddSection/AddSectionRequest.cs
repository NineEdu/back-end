namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.AddSection;

public class AddSectionRequest
{
    public string CourseId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
}
