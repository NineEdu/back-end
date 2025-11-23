namespace ELearningPTIT.Modules.Courses.Domain.Exceptions;

public class LectureNotFoundException : Exception
{
    public LectureNotFoundException(string lectureId)
        : base($"Lecture with ID '{lectureId}' was not found.")
    {
        LectureId = lectureId;
    }

    public string LectureId { get; }
}
