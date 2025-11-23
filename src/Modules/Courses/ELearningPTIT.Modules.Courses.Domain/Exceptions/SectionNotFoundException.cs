namespace ELearningPTIT.Modules.Courses.Domain.Exceptions;

public class SectionNotFoundException : Exception
{
    public SectionNotFoundException(string sectionId)
        : base($"Section with ID '{sectionId}' was not found.")
    {
        SectionId = sectionId;
    }

    public string SectionId { get; }
}
