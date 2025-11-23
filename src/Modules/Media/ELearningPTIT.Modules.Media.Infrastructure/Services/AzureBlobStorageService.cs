using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ELearningPTIT.Modules.Media.Application.Abstractions;
using ELearningPTIT.Modules.Media.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ELearningPTIT.Modules.Media.Infrastructure.Services;

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureBlobStorageSettings _settings;

    public AzureBlobStorageService(IOptions<AzureBlobStorageSettings> settings)
    {
        _settings = settings.Value;
        _blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
    }

    public async Task<BlobUploadResult> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = containerClient.GetBlobClient(fileName);

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType
        };

        content.Position = 0;
        await blobClient.UploadAsync(content, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        }, cancellationToken);

        return new BlobUploadResult(
            fileName,
            blobClient.Uri.ToString(),
            content.Length
        );
    }

    public async Task<Stream?> DownloadAsync(
        string blobName,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            return null;
        }

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }

    public async Task DeleteAsync(
        string blobName,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<string> GetBlobUrlAsync(
        string blobName,
        string containerName,
        TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            throw new InvalidOperationException($"Blob {blobName} does not exist in container {containerName}");
        }

        if (expiresIn.HasValue)
        {
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.Add(expiresIn.Value)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }

        return blobClient.Uri.ToString();
    }

    public async Task<bool> ExistsAsync(
        string blobName,
        string containerName,
        CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.ExistsAsync(cancellationToken);
    }
}
