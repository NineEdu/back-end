using System.Security.Claims;
using ELearningPTIT.Modules.Media.Application.Commands.DeleteMedia;
using FastEndpoints;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.DeleteMedia;

public class DeleteMediaEndpoint(
    Wemogy.CQRS.Commands.Abstractions.ICommandHandler<DeleteMediaCommand> commandHandler
) : Endpoint<DeleteMediaRequest>
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

        await commandHandler.HandleAsync(command);
        await Send.NoContentAsync(ct);
    }
}
