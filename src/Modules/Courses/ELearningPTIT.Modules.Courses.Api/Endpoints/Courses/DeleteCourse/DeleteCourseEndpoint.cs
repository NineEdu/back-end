using ELearningPTIT.Modules.Courses.Application.Commands;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.DeleteCourse;

public class DeleteCourseEndpoint(ICommands commands)
    : Endpoint<DeleteCourseRequest>
{
    public override void Configure()
    {
        Delete("/{courseId}");
        Group<CoursesGroup>();
        // TODO: Add permission-based authorization: courses:delete
    }

    public override async Task HandleAsync(DeleteCourseRequest req, CancellationToken ct)
    {
        var command = new DeleteCourseCommand { CourseId = req.CourseId };
        await commands.RunAsync(command);

        await Send.NoContentAsync(ct);
    }
}
