using Skeleton.Core.Infrastructure.Communications.Email.Models;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public interface IEmailService
{
    Task<EmailSendResult> SendAsync(
        string to,
        string subject,
        string template,
        IReadOnlyDictionary<string, string> templateValues,
        IReadOnlyList<EmailAttachmentRequest>? attachments = null,
        CancellationToken cancellationToken = default);
}
