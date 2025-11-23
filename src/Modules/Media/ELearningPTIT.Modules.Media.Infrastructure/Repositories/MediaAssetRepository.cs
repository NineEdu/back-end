using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;
using MongoDB.Driver;

namespace ELearningPTIT.Modules.Media.Infrastructure.Repositories;

public class MediaAssetRepository : IMediaAssetRepository
{
    private readonly IMongoCollection<MediaAsset> _collection;

    public MediaAssetRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<MediaAsset>("media_assets");

        var indexKeys = Builders<MediaAsset>.IndexKeys
            .Ascending(x => x.UploadedBy)
            .Descending(x => x.CreatedAt);

        _collection.Indexes.CreateOneAsync(new CreateIndexModel<MediaAsset>(indexKeys));
    }

    public async Task<MediaAsset?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<MediaAsset>> GetByUploaderAsync(string uploaderId, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.UploadedBy == uploaderId && x.Status != MediaStatus.Deleted)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MediaAsset>> GetByStatusAsync(MediaStatus status, CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(x => x.Status == status)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<MediaAsset> CreateAsync(MediaAsset mediaAsset, CancellationToken cancellationToken = default)
    {
        mediaAsset.CreatedAt = DateTime.UtcNow;
        mediaAsset.UpdatedAt = DateTime.UtcNow;

        await _collection.InsertOneAsync(mediaAsset, cancellationToken: cancellationToken);
        return mediaAsset;
    }

    public async Task<MediaAsset> UpdateAsync(MediaAsset mediaAsset, CancellationToken cancellationToken = default)
    {
        mediaAsset.UpdatedAt = DateTime.UtcNow;

        await _collection.ReplaceOneAsync(
            x => x.Id == mediaAsset.Id,
            mediaAsset,
            cancellationToken: cancellationToken
        );

        return mediaAsset;
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }
}
