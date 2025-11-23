using ELearningPTIT.Modules.Courses.Application.Commands;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.DeleteLecture;

public class DeleteLectureEndpoint(ICommands commands)
    : Endpoint<DeleteLectureRequest>
{
    public override void Configure()
    {
        Delete("/{CourseId}/sections/{SectionId}/lectures/{LectureId}");
        Group<CoursesGroup>();
    }

    public override async Task HandleAsync(DeleteLectureRequest req, CancellationToken ct)
    {
        var command = new DeleteLectureCommand
        {
            CourseId = req.CourseId,
            SectionId = req.SectionId,
            LectureId = req.LectureId
        };

        await commands.RunAsync(command);
        await Send.NoContentAsync(ct);
    }
}
