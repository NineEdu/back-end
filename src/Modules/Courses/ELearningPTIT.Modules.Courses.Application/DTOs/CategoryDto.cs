namespace ELearningPTIT.Modules.Courses.Application.DTOs;

public class CategoryDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ParentCategoryId { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public int CourseCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
