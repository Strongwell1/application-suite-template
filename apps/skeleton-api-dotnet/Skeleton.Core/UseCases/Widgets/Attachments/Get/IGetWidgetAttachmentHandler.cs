namespace Skeleton.Core.UseCases.Widgets.Attachments.Get;

public interface IGetWidgetAttachmentHandler
{
    Task<GetWidgetAttachmentResult?> Handle(
        GetWidgetAttachmentQuery query,
        CancellationToken cancellationToken);
}
