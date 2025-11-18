namespace ELearningPTIT.Modules.Courses.Domain.Exceptions;

/// <summary>
/// Exception thrown when a user attempts unauthorized access to a course
/// </summary>
public class UnauthorizedCourseAccessException : Exception
{
    public UnauthorizedCourseAccessException(string userId, string courseId)
        : base($"User '{userId}' is not authorized to access course '{courseId}'.")
    {
        UserId = userId;
        CourseId = courseId;
    }

    public string UserId { get; }
    public string CourseId { get; }
}
