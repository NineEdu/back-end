using System.Security.Claims;
using ELearningPTIT.Modules.Media.Application.Commands;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.DeleteMedia;

public class DeleteMediaEndpoint(ICommands commands)
    : Endpoint<DeleteMediaRequest>
{
    public override void Configure()
    {
        Delete("/{Id}");
        Group<MediaGroup>();
    }

    public override async Task HandleAsync(DeleteMediaRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var command = new DeleteMediaCommand
        {
            MediaAssetId = req.Id,
            RequestedBy = userId
        };

        await commands.RunAsync(command);
        await Send.NoContentAsync(ct);
    }
}
