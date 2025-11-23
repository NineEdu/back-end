using ELearningPTIT.Modules.Media.Application.Abstractions;
using ELearningPTIT.Modules.Media.Domain.Repositories;
using Wemogy.CQRS.Commands.Abstractions;

namespace ELearningPTIT.Modules.Media.Application.Commands.DeleteMedia;

public class DeleteMediaCommandHandler(
    IMediaAssetRepository mediaAssetRepository,
    IBlobStorageService blobStorageService
) : ICommandHandler<DeleteMediaCommand>
{
    public async Task HandleAsync(DeleteMediaCommand command)
    {
        var mediaAsset = await mediaAssetRepository.GetByIdAsync(command.MediaAssetId)
            ?? throw new InvalidOperationException($"Media asset with ID {command.MediaAssetId} not found");

        if (mediaAsset.UploadedBy != command.RequestedBy)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this media asset");
        }

        await blobStorageService.DeleteAsync(mediaAsset.BlobName, mediaAsset.ContainerName);

        foreach (var variant in mediaAsset.Variants)
        {
            await blobStorageService.DeleteAsync(variant.BlobName, mediaAsset.ContainerName);
        }

        await mediaAssetRepository.DeleteAsync(command.MediaAssetId);
    }
}
