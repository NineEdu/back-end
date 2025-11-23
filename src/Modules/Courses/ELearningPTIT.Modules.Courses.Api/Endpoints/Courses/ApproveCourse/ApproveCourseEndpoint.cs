using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using System.Security.Claims;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.ApproveCourse;

public class ApproveCourseEndpoint(ICommands commands)
    : Endpoint<ApproveCourseRequest, CourseDto>
{
    public override void Configure()
    {
        Post("/{CourseId}/approve");
        Group<CoursesGroup>();
        // TODO: Restrict to Admin role
    }

    public override async Task HandleAsync(ApproveCourseRequest req, CancellationToken ct)
    {
        var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User not authenticated");

        var command = new ApproveCourseCommand
        {
            CourseId = req.CourseId,
            ApprovedBy = adminId
        };

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
