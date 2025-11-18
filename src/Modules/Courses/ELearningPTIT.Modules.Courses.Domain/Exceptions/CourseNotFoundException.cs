namespace ELearningPTIT.Modules.Courses.Domain.Exceptions;

/// <summary>
/// Exception thrown when a course is not found
/// </summary>
public class CourseNotFoundException : Exception
{
    public CourseNotFoundException(string courseId)
        : base($"Course with ID '{courseId}' was not found.")
    {
        CourseId = courseId;
    }

    public string CourseId { get; }
}
