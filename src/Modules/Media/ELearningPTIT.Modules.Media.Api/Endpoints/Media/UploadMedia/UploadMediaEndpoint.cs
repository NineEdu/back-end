using System.Security.Claims;
using ELearningPTIT.Modules.Media.Application.Commands;
using ELearningPTIT.Modules.Media.Application.DTOs;
using FastEndpoints;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.UploadMedia;

public class UploadMediaEndpoint(ICommands commands)
    : Endpoint<UploadMediaRequest, MediaAssetDto>
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

        var result = await commands.RunAsync(command);
        await Send.ResponseAsync(result, 201, ct);
    }
}
