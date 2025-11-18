namespace ELearningPTIT.Modules.Courses.Domain.ValueObjects;

/// <summary>
/// Represents the publication status of a course
/// </summary>
public enum CourseStatus
{
    /// <summary>
    /// Course is being created/edited by instructor
    /// </summary>
    Draft = 1,

    /// <summary>
    /// Course submitted for admin approval
    /// </summary>
    PendingApproval = 2,

    /// <summary>
    /// Course approved and visible to students
    /// </summary>
    Published = 3,

    /// <summary>
    /// Course rejected by admin (needs revision)
    /// </summary>
    Rejected = 4,

    /// <summary>
    /// Course archived/hidden from public view
    /// </summary>
    Archived = 5
}
