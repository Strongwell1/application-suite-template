namespace Skeleton.Core.Infrastructure.Storage;

public interface IBlobStorageService
{
    Task UploadAsync(string blobPath, Stream content, string contentType, CancellationToken cancellationToken);
    Task<Stream> DownloadAsync(string blobPath, CancellationToken cancellationToken);
}
