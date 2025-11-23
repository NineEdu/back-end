using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class SubmitCourseForApprovalCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<SubmitCourseForApprovalCommand, CourseDto>
{
    public async Task<CourseDto> HandleAsync(SubmitCourseForApprovalCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        if (!course.CanBePublished())
        {
            throw new InvalidOperationException("Course must have at least one section with lectures before submission.");
        }

        course.Status = CourseStatus.PendingApproval;
        course.SubmittedForApprovalAt = DateTime.UtcNow;
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        return course.Adapt<CourseDto>();
    }
}
