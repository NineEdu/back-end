using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;

namespace ELearningPTIT.Modules.Media.Application.DTOs;

public record MediaAssetDto(
    string Id,
    string FileName,
    string OriginalFileName,
    string ContentType,
    MediaType MediaType,
    long FileSize,
    string BlobUrl,
    MediaStatus Status,
    string UploadedBy,
    TimeSpan? Duration,
    int? Width,
    int? Height,
    Dictionary<string, string> Metadata,
    List<MediaVariantDto> Variants,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record MediaVariantDto(
    string Quality,
    string BlobUrl,
    long FileSize,
    int? Width,
    int? Height
);

public static class MediaAssetExtensions
{
    public static MediaAssetDto ToDto(this MediaAsset asset)
    {
        return new MediaAssetDto(
            asset.Id,
            asset.FileName,
            asset.OriginalFileName,
            asset.ContentType,
            asset.MediaType,
            asset.FileSize,
            asset.BlobUrl,
            asset.Status,
            asset.UploadedBy,
            asset.Duration,
            asset.Width,
            asset.Height,
            asset.Metadata,
            asset.Variants.Select(v => new MediaVariantDto(
                v.Quality,
                v.BlobUrl,
                v.FileSize,
                v.Width,
                v.Height
            )).ToList(),
            asset.CreatedAt,
            asset.UpdatedAt
        );
    }
}
