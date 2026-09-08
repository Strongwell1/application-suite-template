namespace Skeleton.Core.UseCases.Widgets.Attachments.Get;

public sealed record GetWidgetAttachmentQuery
{
    public required Guid WidgetId { get; init; }
    public required Guid AttachmentId { get; init; }
}
