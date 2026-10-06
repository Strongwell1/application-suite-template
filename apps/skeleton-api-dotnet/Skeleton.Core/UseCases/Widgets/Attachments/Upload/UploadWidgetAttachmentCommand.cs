namespace Skeleton.Core.UseCases.Widgets.Attachments.Upload;

public sealed record UploadWidgetAttachmentCommand
{
    public required Guid WidgetId { get; init; }
    public required Guid AttachmentId { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required long FileSizeBytes { get; init; }
    public required Stream Content { get; init; }
}
