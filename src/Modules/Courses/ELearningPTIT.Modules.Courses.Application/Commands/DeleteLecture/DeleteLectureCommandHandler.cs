using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class DeleteLectureCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<DeleteLectureCommand>
{
    public async Task HandleAsync(DeleteLectureCommand command)
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

        var lecture = section.Lectures.FirstOrDefault(l => l.Id == command.LectureId);
        if (lecture == null)
        {
            throw new LectureNotFoundException(command.LectureId);
        }

        section.Lectures.Remove(lecture);
        course.CalculateTotals();
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);
    }
}
