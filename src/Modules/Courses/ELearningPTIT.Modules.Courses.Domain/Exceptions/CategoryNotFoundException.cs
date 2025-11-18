namespace ELearningPTIT.Modules.Courses.Domain.Exceptions;

/// <summary>
/// Exception thrown when a category is not found
/// </summary>
public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException(string categoryId)
        : base($"Category with ID '{categoryId}' was not found.")
    {
        CategoryId = categoryId;
    }

    public string CategoryId { get; }
}
