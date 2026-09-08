namespace Skeleton.Core.UseCases.Widgets.Attachments.Upload;

public interface IUploadWidgetAttachmentHandler
{
    Task<UploadWidgetAttachmentResult> Handle(
        UploadWidgetAttachmentCommand command,
        CancellationToken cancellationToken);
}
