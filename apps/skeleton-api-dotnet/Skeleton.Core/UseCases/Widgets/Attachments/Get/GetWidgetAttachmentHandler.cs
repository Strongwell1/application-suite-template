namespace Skeleton.Core.UseCases.Widgets.Attachments.Get;

public sealed class GetWidgetAttachmentHandler : IGetWidgetAttachmentHandler
{
    public Task<GetWidgetAttachmentResult?> Handle(
        GetWidgetAttachmentQuery query,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
