namespace Skeleton.Core.UseCases.Widgets.Attachments.Upload;

public sealed class UploadWidgetAttachmentHandler : IUploadWidgetAttachmentHandler
{
    public Task<UploadWidgetAttachmentResult> Handle(
        UploadWidgetAttachmentCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
