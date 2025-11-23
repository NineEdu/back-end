using ELearningPTIT.Modules.Media.Application.DTOs;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Queries.GetMediaAssets;

public class GetMediaAssetsQueryHandler(
    IMediaAssetRepository mediaAssetRepository
) : IQueryHandler<GetMediaAssetsQuery, IEnumerable<MediaAssetDto>>
{
    public async Task<IEnumerable<MediaAssetDto>> HandleAsync(GetMediaAssetsQuery query, CancellationToken cancellationToken = default)
    {
        var mediaAssets = await mediaAssetRepository.GetByUploaderAsync(query.UploaderId, cancellationToken);
        return mediaAssets.Select(a => a.ToDto());
    }
}
