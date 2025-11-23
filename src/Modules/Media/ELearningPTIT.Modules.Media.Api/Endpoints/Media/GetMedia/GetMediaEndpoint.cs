using ELearningPTIT.Modules.Media.Application.DTOs;
using ELearningPTIT.Modules.Media.Application.Queries;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.GetMedia;

public class GetMediaEndpoint(IQueries queries)
    : Endpoint<GetMediaRequest, MediaAssetDto>
{
    public override void Configure()
    {
        Get("/{Id}");
        Group<MediaGroup>();
    }

    public override async Task HandleAsync(GetMediaRequest req, CancellationToken ct)
    {
        var query = new GetMediaAssetQuery { MediaAssetId = req.Id };
        var result = await queries.QueryAsync(query, ct);

        if (result == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.ResponseAsync(result, 200, ct);
    }
}
