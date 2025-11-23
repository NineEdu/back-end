using ELearningPTIT.Modules.Media.Application.Abstractions;
using ELearningPTIT.Modules.Media.Application.Queries.GetMediaAsset;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;
using ELearningPTIT.Modules.Media.Application.DTOs;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.GetMediaUrl;

public class GetMediaUrlEndpoint(
    IQueryHandler<GetMediaAssetQuery, MediaAssetDto?> queryHandler,
    IBlobStorageService blobStorageService
) : Endpoint<GetMediaUrlRequest, GetMediaUrlResponse>
{
    public override void Configure()
    {
        Get("/{Id}/url");
        Group<MediaGroup>();
    }

    public override async Task HandleAsync(GetMediaUrlRequest req, CancellationToken ct)
    {
        var query = new GetMediaAssetQuery
        {
            MediaAssetId = req.Id
        };

        var mediaAsset = await queryHandler.HandleAsync(query, ct);

        if (mediaAsset == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var expiresIn = req.ExpiresInMinutes.HasValue
            ? TimeSpan.FromMinutes(req.ExpiresInMinutes.Value)
            : TimeSpan.FromHours(1);

        var url = await blobStorageService.GetBlobUrlAsync(
            mediaAsset.FileName,
            "media",
            expiresIn,
            ct
        );

        var response = new GetMediaUrlResponse
        {
            Url = url,
            ExpiresAt = DateTime.UtcNow.Add(expiresIn)
        };

        await Send.ResponseAsync(response, 200, ct);
    }
}
