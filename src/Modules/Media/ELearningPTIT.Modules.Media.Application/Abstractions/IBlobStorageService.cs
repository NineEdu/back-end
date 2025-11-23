namespace ELearningPTIT.Modules.Media.Application.Abstractions;

public interface IBlobStorageService
{
    Task<BlobUploadResult> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        string containerName,
        CancellationToken cancellationToken = default);

    Task<Stream?> DownloadAsync(
        string blobName,
        string containerName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string blobName,
        string containerName,
        CancellationToken cancellationToken = default);

    Task<string> GetBlobUrlAsync(
        string blobName,
        string containerName,
        TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        string blobName,
        string containerName,
        CancellationToken cancellationToken = default);
}

public record BlobUploadResult(
    string BlobName,
    string BlobUrl,
    long FileSize
);
