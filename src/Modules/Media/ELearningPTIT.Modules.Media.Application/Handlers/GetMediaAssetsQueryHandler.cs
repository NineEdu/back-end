using ELearningPTIT.Modules.Media.Application.DTOs;
using ELearningPTIT.Modules.Media.Application.Queries;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;
using Wemogy.CQRS.Queries.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Handlers;

public class GetMediaAssetsQueryHandler(
    IMediaAssetRepository mediaAssetRepository
) : IQueryHandler<GetMediaAssetsQuery, IEnumerable<MediaAssetDto>>
{
    public async Task<IEnumerable<MediaAssetDto>> HandleAsync(GetMediaAssetsQuery query, CancellationToken cancellationToken = default)
    {
        var mediaAssets = await mediaAssetRepository.QueryAsync(
            x => x.UploadedBy == query.UploaderId && x.Status != MediaStatus.Deleted,
            cancellationToken
        );

        return mediaAssets.Select(a => a.ToDto());
    }
}
