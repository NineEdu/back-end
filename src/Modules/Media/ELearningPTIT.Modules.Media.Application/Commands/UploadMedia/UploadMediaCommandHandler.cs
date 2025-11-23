using ELearningPTIT.Modules.Media.Application.Abstractions;
using ELearningPTIT.Modules.Media.Application.DTOs;
using ELearningPTIT.Modules.Media.Domain.Entities;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using ELearningPTIT.Modules.Media.Domain.ValueObjects;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Commands.UploadMedia;

public class UploadMediaCommandHandler(
    IMediaAssetRepository mediaAssetRepository,
    IBlobStorageService blobStorageService
) : ICommandHandler<UploadMediaCommand, MediaAssetDto>
{
    private const string ContainerName = "media";

    public async Task<MediaAssetDto> HandleAsync(UploadMediaCommand command)
    {
        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(command.FileName)}";

        var uploadResult = await blobStorageService.UploadAsync(
            command.FileStream,
            uniqueFileName,
            command.ContentType,
            ContainerName
        );

        var mediaAsset = new MediaAsset
        {
            FileName = uniqueFileName,
            OriginalFileName = command.FileName,
            ContentType = command.ContentType,
            MediaType = MediaAsset.GetMediaTypeFromContentType(command.ContentType),
            FileSize = command.FileSize,
            BlobUrl = uploadResult.BlobUrl,
            ContainerName = ContainerName,
            BlobName = uploadResult.BlobName,
            Status = MediaStatus.Ready,
            UploadedBy = command.UploadedBy,
            Metadata = command.Metadata ?? new Dictionary<string, string>()
        };

        await mediaAssetRepository.CreateAsync(mediaAsset);

        return mediaAsset.ToDto();
    }
}
