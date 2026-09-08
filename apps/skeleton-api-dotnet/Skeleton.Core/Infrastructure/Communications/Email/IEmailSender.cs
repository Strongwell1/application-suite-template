using Skeleton.Core.Infrastructure.Communications.Email.Models;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public interface IEmailSender
{
    Task<EmailSendResult> SendAsync(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default);
}
