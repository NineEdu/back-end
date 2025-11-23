using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.SubmitForApproval;

public class SubmitForApprovalEndpoint(ICommands commands)
    : Endpoint<SubmitForApprovalRequest, CourseDto>
{
    public override void Configure()
    {
        Post("/{CourseId}/submit");
        Group<CoursesGroup>();
    }

    public override async Task HandleAsync(SubmitForApprovalRequest req, CancellationToken ct)
    {
        var command = new SubmitCourseForApprovalCommand
        {
            CourseId = req.CourseId
        };

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
