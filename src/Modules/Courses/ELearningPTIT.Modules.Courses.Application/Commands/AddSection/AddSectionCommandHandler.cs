using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Entities;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class AddSectionCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<AddSectionCommand, SectionDto>
{
    public async Task<SectionDto> HandleAsync(AddSectionCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        var section = new Section
        {
            Title = command.Title,
            Description = command.Description,
            Order = command.Order > 0 ? command.Order : course.Sections.Count + 1
        };

        course.Sections.Add(section);
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        return section.Adapt<SectionDto>();
    }
}
