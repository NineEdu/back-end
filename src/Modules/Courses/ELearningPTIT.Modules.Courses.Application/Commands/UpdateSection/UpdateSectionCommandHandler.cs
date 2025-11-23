using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class UpdateSectionCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<UpdateSectionCommand, SectionDto>
{
    public async Task<SectionDto> HandleAsync(UpdateSectionCommand command)
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

        section.Title = command.Title;
        section.Description = command.Description;
        section.Order = command.Order;

        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        return section.Adapt<SectionDto>();
    }
}
