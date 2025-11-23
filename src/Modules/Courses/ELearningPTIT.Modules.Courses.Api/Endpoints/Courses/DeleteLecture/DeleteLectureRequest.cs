namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.DeleteLecture;

public class DeleteLectureRequest
{
    public string CourseId { get; set; } = string.Empty;
    public string SectionId { get; set; } = string.Empty;
    public string LectureId { get; set; } = string.Empty;
}
