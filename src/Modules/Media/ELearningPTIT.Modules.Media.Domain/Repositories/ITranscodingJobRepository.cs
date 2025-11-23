using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Media.Domain.Repositories;

public interface ITranscodingJobRepository
{
    Task<TranscodingJob?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<TranscodingJob?> GetByMediaAssetIdAsync(string mediaAssetId, CancellationToken cancellationToken = default);

    Task<IEnumerable<TranscodingJob>> GetByStatusAsync(TranscodingStatus status, CancellationToken cancellationToken = default);

    Task<TranscodingJob> CreateAsync(TranscodingJob job, CancellationToken cancellationToken = default);

    Task<TranscodingJob> UpdateAsync(TranscodingJob job, CancellationToken cancellationToken = default);
}
