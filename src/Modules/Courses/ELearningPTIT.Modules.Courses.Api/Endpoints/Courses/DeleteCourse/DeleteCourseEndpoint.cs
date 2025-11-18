using ELearningPTIT.Modules.Courses.Application.Commands;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.DeleteCourse;

public class DeleteCourseEndpoint(
    Wemogy.CQRS.Commands.Abstractions.ICommandHandler<DeleteCourseCommand> commandHandler)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/{courseId}");
        Group<CoursesGroup>();
        // TODO: Add permission-based authorization: courses:delete
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var courseId = Route<string>("courseId")!;

        var command = new DeleteCourseCommand { CourseId = courseId };
        await commandHandler.HandleAsync(command);

        await Send.NoContentAsync(ct);
    }
}
