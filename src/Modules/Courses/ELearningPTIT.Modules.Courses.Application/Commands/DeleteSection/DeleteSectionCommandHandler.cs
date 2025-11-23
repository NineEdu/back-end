using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class DeleteSectionCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<DeleteSectionCommand>
{
    public async Task HandleAsync(DeleteSectionCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        var section = course.Sections.FirstOrDefault(s => s.Id == command.SectionId);
        if (section == null)
        {
            throw new SectionNotFoundException(command.SectionId);
        }

        course.Sections.Remove(section);
        course.CalculateTotals();
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);
    }
}
