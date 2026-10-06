using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Skeleton.Core.Infrastructure.Storage;

namespace Skeleton.Api.Infrastructure.Storage;

public sealed class BlobStorageService(BlobServiceClient blobServiceClient, string containerName)
    : IBlobStorageService
{
    public async Task UploadAsync(
        string blobPath,
        Stream content,
        string contentType,
        CancellationToken cancellationToken)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobPath);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
    }

    public async Task<Stream> DownloadAsync(string blobPath, CancellationToken cancellationToken)
    {
        var container = blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(blobPath);
        var response = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }
}
