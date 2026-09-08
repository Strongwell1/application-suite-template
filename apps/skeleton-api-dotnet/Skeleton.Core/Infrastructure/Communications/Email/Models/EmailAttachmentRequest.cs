namespace Skeleton.Core.Infrastructure.Communications.Email.Models;

public sealed record EmailAttachmentRequest
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Content { get; init; }
}
