namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Categories.CreateCategory;

public class CreateCategoryRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ParentCategoryId { get; set; }
    public int DisplayOrder { get; set; } = 0;
}
