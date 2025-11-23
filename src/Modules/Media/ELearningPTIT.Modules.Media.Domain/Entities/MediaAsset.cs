using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YourCompany.YourProject.Shared.Core.Abstractions;
using YourCompany.YourProject.Shared.Core.Attributes;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Media.Domain.Entities;

[CollectionName("media_assets")]
public class MediaAsset : EntityBase
{
    public MediaAsset()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    [BsonElement("fileName")]
    [BsonRequired]
    public string FileName { get; set; } = string.Empty;

    [BsonElement("originalFileName")]
    public string OriginalFileName { get; set; } = string.Empty;

    [BsonElement("contentType")]
    [BsonRequired]
    public string ContentType { get; set; } = string.Empty;

    [BsonElement("mediaType")]
    public MediaType MediaType { get; set; }

    [BsonElement("fileSize")]
    public long FileSize { get; set; }

    [BsonElement("blobUrl")]
    public string BlobUrl { get; set; } = string.Empty;

    [BsonElement("containerName")]
    public string ContainerName { get; set; } = string.Empty;

    [BsonElement("blobName")]
    public string BlobName { get; set; } = string.Empty;

    [BsonElement("status")]
    public MediaStatus Status { get; set; } = MediaStatus.Pending;

    [BsonElement("uploadedBy")]
    [BsonRequired]
    public string UploadedBy { get; set; } = string.Empty;

    [BsonElement("duration")]
    public TimeSpan? Duration { get; set; }

    [BsonElement("width")]
    public int? Width { get; set; }

    [BsonElement("height")]
    public int? Height { get; set; }

    [BsonElement("metadata")]
    public Dictionary<string, string> Metadata { get; set; } = new();

    [BsonElement("transcodingJobId")]
    public string? TranscodingJobId { get; set; }

    [BsonElement("variants")]
    public List<MediaVariant> Variants { get; set; } = new();

    public static MediaType GetMediaTypeFromContentType(string contentType)
    {
        if (contentType.StartsWith("video/"))
            return MediaType.Video;
        if (contentType.StartsWith("audio/"))
            return MediaType.Audio;
        if (contentType.StartsWith("image/"))
            return MediaType.Image;
        if (contentType == "application/pdf" ||
            contentType.StartsWith("application/msword") ||
            contentType.StartsWith("application/vnd.openxmlformats"))
            return MediaType.Document;

        return MediaType.Other;
    }
}

public class MediaVariant
{
    [BsonElement("quality")]
    public string Quality { get; set; } = string.Empty;

    [BsonElement("blobName")]
    public string BlobName { get; set; } = string.Empty;

    [BsonElement("blobUrl")]
    public string BlobUrl { get; set; } = string.Empty;

    [BsonElement("fileSize")]
    public long FileSize { get; set; }

    [BsonElement("width")]
    public int? Width { get; set; }

    [BsonElement("height")]
    public int? Height { get; set; }
}
