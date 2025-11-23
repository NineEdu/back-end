using ELearningPTIT.Modules.Courses.Application.Commands;
using ELearningPTIT.Modules.Courses.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Courses.Api.Endpoints.Courses.AddSection;

public class AddSectionEndpoint(ICommands commands)
    : Endpoint<AddSectionRequest, SectionDto>
{
    public override void Configure()
    {
        Post("/{CourseId}/sections");
        Group<CoursesGroup>();
    }

    public override async Task HandleAsync(AddSectionRequest req, CancellationToken ct)
    {
        var command = new AddSectionCommand
        {
            CourseId = req.CourseId,
            Title = req.Title,
            Description = req.Description,
            Order = req.Order
        };

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 201, ct);
    }
}
