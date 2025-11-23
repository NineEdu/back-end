using ELearningPTIT.Modules.Media.Application.DTOs;
using ELearningPTIT.Modules.Media.Application.Queries;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Handlers;

public class GetMediaAssetQueryHandler(
    IMediaAssetRepository mediaAssetRepository
) : IQueryHandler<GetMediaAssetQuery, MediaAssetDto?>
{
    public async Task<MediaAssetDto?> HandleAsync(GetMediaAssetQuery query, CancellationToken cancellationToken = default)
    {
        var mediaAsset = await mediaAssetRepository.GetAsync(query.MediaAssetId, cancellationToken);
        return mediaAsset?.ToDto();
    }
}
