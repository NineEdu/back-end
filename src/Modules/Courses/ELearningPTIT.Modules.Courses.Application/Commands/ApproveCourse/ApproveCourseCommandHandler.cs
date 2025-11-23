using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class ApproveCourseCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<ApproveCourseCommand, CourseDto>
{
    public async Task<CourseDto> HandleAsync(ApproveCourseCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        if (course.Status != CourseStatus.PendingApproval)
        {
            throw new InvalidOperationException("Only courses pending approval can be approved.");
        }

        course.Status = CourseStatus.Published;
        course.ApprovedBy = command.ApprovedBy;
        course.ApprovedAt = DateTime.UtcNow;
        course.PublishedAt = DateTime.UtcNow;
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        // TODO: Send notification to instructor (will be implemented with Notification Service)

        return course.Adapt<CourseDto>();
    }
}
