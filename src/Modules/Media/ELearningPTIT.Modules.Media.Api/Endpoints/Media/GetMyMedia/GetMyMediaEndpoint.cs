using System.Security.Claims;
using ELearningPTIT.Modules.Media.Application.DTOs;
using ELearningPTIT.Modules.Media.Application.Queries;
using FastEndpoints;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Api.Endpoints.Media.GetMyMedia;

public class GetMyMediaEndpoint(IQueries queries)
    : EndpointWithoutRequest<IEnumerable<MediaAssetDto>>
{
    public override void Configure()
    {
        Get("/my");
        Group<MediaGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var query = new GetMediaAssetsQuery { UploaderId = userId };
        var result = await queries.QueryAsync(query, ct);
        await Send.ResponseAsync(result, 200, ct);
    }
}
