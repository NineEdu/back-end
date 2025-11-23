using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.UpdateSection;

public class UpdateSectionEndpoint(ICommands commands)
    : Endpoint<UpdateSectionRequest, SectionDto>
{
    public override void Configure()
    {
        Put("/{CourseId}/sections/{SectionId}");
        Group<CoursesGroup>();
    }

    public override async Task HandleAsync(UpdateSectionRequest req, CancellationToken ct)
    {
        var command = new UpdateSectionCommand
        {
            CourseId = req.CourseId,
            SectionId = req.SectionId,
            Title = req.Title,
            Description = req.Description,
            Order = req.Order
        };

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 200, ct);
    }
}
