using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using System.Security.Claims;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.RejectCourse;

public class RejectCourseEndpoint(ICommands commands)
    : Endpoint<RejectCourseRequest, CourseDto>
{
    public override void Configure()
    {
        Post("/{CourseId}/reject");
        Group<CoursesGroup>();
        // TODO: Restrict to Admin role
    }

    public override async Task HandleAsync(RejectCourseRequest req, CancellationToken ct)
    {
        var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User not authenticated");

        var command = new RejectCourseCommand
        {
            CourseId = req.CourseId,
            RejectedBy = adminId,
            RejectionReason = req.Reason
        };

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
