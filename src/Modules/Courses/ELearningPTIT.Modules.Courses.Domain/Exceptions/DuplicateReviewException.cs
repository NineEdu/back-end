namespace ELearningPTIT.Modules.Courses.Domain.Exceptions;

/// <summary>
/// Exception thrown when a user tries to create multiple reviews for the same course
/// </summary>
public class DuplicateReviewException : Exception
{
    public DuplicateReviewException(string userId, string courseId)
        : base($"User '{userId}' has already reviewed course '{courseId}'.")
    {
        UserId = userId;
        CourseId = courseId;
    }

    public string UserId { get; }
    public string CourseId { get; }
}
