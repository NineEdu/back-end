using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;
using MongoDB.Driver;

namespace ELearningPTIT.Modules.Media.Infrastructure.Repositories;

public class TranscodingJobRepository : ITranscodingJobRepository
{
    private readonly IMongoCollection<TranscodingJob> _collection;

    public TranscodingJobRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<TranscodingJob>("transcoding_jobs");

        var indexKeys = Builders<TranscodingJob>.IndexKeys
            .Ascending(x => x.MediaAssetId);

        _collection.Indexes.CreateOneAsync(new CreateIndexModel<TranscodingJob>(indexKeys));
    }

    public async Task<TranscodingJob?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TranscodingJob?> GetByMediaAssetIdAsync(string mediaAssetId, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.MediaAssetId == mediaAssetId)
            .SortByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TranscodingJob>> GetByStatusAsync(TranscodingStatus status, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.Status == status)
            .SortBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<TranscodingJob> CreateAsync(TranscodingJob job, CancellationToken cancellationToken = default)
    {
        job.CreatedAt = DateTime.UtcNow;
        job.UpdatedAt = DateTime.UtcNow;

        await _collection.InsertOneAsync(job, cancellationToken: cancellationToken);
        return job;
    }

    public async Task<TranscodingJob> UpdateAsync(TranscodingJob job, CancellationToken cancellationToken = default)
    {
        job.UpdatedAt = DateTime.UtcNow;

        await _collection.ReplaceOneAsync(
            x => x.Id == job.Id,
            job,
            cancellationToken: cancellationToken
        );

        return job;
    }
}
