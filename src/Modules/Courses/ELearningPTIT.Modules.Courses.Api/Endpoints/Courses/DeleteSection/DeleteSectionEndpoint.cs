using ELearningPTIT.Modules.Courses.Application.Commands;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.DeleteSection;

public class DeleteSectionEndpoint(ICommands commands)
    : Endpoint<DeleteSectionRequest>
{
    public override void Configure()
    {
        Delete("/{CourseId}/sections/{SectionId}");
        Group<CoursesGroup>();
    }

    public override async Task HandleAsync(DeleteSectionRequest req, CancellationToken ct)
    {
        var command = new DeleteSectionCommand
        {
            CourseId = req.CourseId,
            SectionId = req.SectionId
        };

        await commands.RunAsync(command);
        await Send.NoContentAsync(ct);
    }
}
