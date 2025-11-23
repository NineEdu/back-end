using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Media.Domain.Entities;

[CollectionName("transcoding_jobs")]
public class TranscodingJob : EntityBase
{
    public TranscodingJob()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonElement("mediaAssetId")]
    [BsonRequired]
    public string MediaAssetId { get; set; } = string.Empty;

    [BsonElement("status")]
    public TranscodingStatus Status { get; set; } = TranscodingStatus.Queued;

    [BsonElement("progress")]
    public int Progress { get; set; } = 0;

    [BsonElement("errorMessage")]
    public string? ErrorMessage { get; set; }

    [BsonElement("startedAt")]
    public DateTime? StartedAt { get; set; }

    [BsonElement("completedAt")]
    public DateTime? CompletedAt { get; set; }

    [BsonElement("outputFormats")]
    public List<string> OutputFormats { get; set; } = new() { "360p", "480p", "720p", "1080p" };

    [BsonElement("completedFormats")]
    public List<string> CompletedFormats { get; set; } = new();
}
