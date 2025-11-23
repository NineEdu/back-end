using System.Security.Claims;
using ELearningPTIT.Modules.Media.Application.Commands.UploadMedia;
using ELearningPTIT.Modules.Media.Application.DTOs;
using FastEndpoints;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.UploadMedia;

public class UploadMediaEndpoint(
    Wemogy.CQRS.Commands.Abstractions.ICommandHandler<UploadMediaCommand, MediaAssetDto> commandHandler
) : Endpoint<UploadMediaRequest, MediaAssetDto>
{
    public override void Configure()
    {
        Post("/upload");
        Group<MediaGroup>();
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadMediaRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        using var stream = new MemoryStream();
        await req.File.CopyToAsync(stream, ct);

        var command = new UploadMediaCommand
        {
            FileStream = stream,
            FileName = req.File.FileName,
            ContentType = req.File.ContentType,
            FileSize = req.File.Length,
            UploadedBy = userId
        };

        var result = await commandHandler.HandleAsync(command);
        await Send.ResponseAsync(result, 201, ct);
    }
}
