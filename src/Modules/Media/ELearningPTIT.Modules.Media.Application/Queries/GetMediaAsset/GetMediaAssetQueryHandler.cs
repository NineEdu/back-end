using ELearningPTIT.Modules.Media.Application.DTOs;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Queries.GetMediaAsset;

public class GetMediaAssetQueryHandler(
    IMediaAssetRepository mediaAssetRepository
) : IQueryHandler<GetMediaAssetQuery, MediaAssetDto?>
{
    public async Task<MediaAssetDto?> HandleAsync(GetMediaAssetQuery query, CancellationToken cancellationToken = default)
    {
        var mediaAsset = await mediaAssetRepository.GetByIdAsync(query.MediaAssetId, cancellationToken);
        return mediaAsset?.ToDto();
    }
}
