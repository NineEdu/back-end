using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using ELearningPTIT.Modules.Courses.Domain.Exceptions;
using ELearningPTIT.Modules.Courses.Domain.Repositories;
using ELearningPTIT.Modules.Courses.Domain.ValueObjects;
using Mapster;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Application.Handlers;

public class RejectCourseCommandHandler(
    ICourseRepository courseRepository
) : ICommandHandler<RejectCourseCommand, CourseDto>
{
    public async Task<CourseDto> HandleAsync(RejectCourseCommand command)
    {
        var course = await courseRepository.GetAsync(command.CourseId);
        if (course == null)
        {
            throw new CourseNotFoundException(command.CourseId);
        }

        if (course.Status != CourseStatus.PendingApproval)
        {
            throw new InvalidOperationException("Only courses pending approval can be rejected.");
        }

        course.Status = CourseStatus.Rejected;
        course.RejectedBy = command.RejectedBy;
        course.RejectedAt = DateTime.UtcNow;
        course.RejectionReason = command.RejectionReason;
        course.LastUpdatedAt = DateTime.UtcNow;

        await courseRepository.ReplaceAsync(course);

        // TODO: Send notification to instructor with rejection reason (will be implemented with Notification Service)

        return course.Adapt<CourseDto>();
    }
}
