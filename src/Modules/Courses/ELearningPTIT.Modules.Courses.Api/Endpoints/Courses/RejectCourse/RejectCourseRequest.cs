namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.RejectCourse;

public class RejectCourseRequest
{
    public string CourseId { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}
