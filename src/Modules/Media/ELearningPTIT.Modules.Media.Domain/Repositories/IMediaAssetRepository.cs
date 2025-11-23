using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Media.Domain.Repositories;

public interface IMediaAssetRepository
{
    Task<MediaAsset?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<IEnumerable<MediaAsset>> GetByUploaderAsync(string uploaderId, CancellationToken cancellationToken = default);

    Task<IEnumerable<MediaAsset>> GetByStatusAsync(MediaStatus status, CancellationToken cancellationToken = default);

    Task<MediaAsset> CreateAsync(MediaAsset mediaAsset, CancellationToken cancellationToken = default);

    Task<MediaAsset> UpdateAsync(MediaAsset mediaAsset, CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}
