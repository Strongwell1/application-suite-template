namespace Skeleton.Core.UseCases.Widgets.Attachments.Get;

public sealed record GetWidgetAttachmentResult
{
    public required string BlobPath { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
}
