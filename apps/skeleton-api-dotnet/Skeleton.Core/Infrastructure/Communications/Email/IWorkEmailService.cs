using Skeleton.Core.Infrastructure.Communications.Email.Models;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public interface IWorkEmailService
{
    Task<EmailSendResult> SendWorkAssignedAsync(
        WorkAssignedEmail email,
        CancellationToken cancellationToken = default);

    Task<EmailSendResult> SendWorkCompletedAsync(
        WorkCompletedEmail email,
        CancellationToken cancellationToken = default);
}
