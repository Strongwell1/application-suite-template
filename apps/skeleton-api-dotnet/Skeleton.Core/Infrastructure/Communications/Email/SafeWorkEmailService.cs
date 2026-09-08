using Skeleton.Core.Infrastructure.Communications.Email.Models;
using Microsoft.Extensions.Logging;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed class SafeWorkEmailService(
    Func<WorkEmailService> innerFactory,
    ILogger<SafeWorkEmailService> logger)
    : IWorkEmailService
{
    public Task<EmailSendResult> SendWorkAssignedAsync(
        WorkAssignedEmail email,
        CancellationToken cancellationToken = default)
    {
        return TrySendAsync(
            () => innerFactory().SendWorkAssignedAsync(email, cancellationToken),
            "work assigned",
            email.WorkItemId,
            cancellationToken);
    }

    public Task<EmailSendResult> SendWorkCompletedAsync(
        WorkCompletedEmail email,
        CancellationToken cancellationToken = default)
    {
        return TrySendAsync(
            () => innerFactory().SendWorkCompletedAsync(email, cancellationToken),
            "work completed",
            email.WorkItemId,
            cancellationToken);
    }

    private async Task<EmailSendResult> TrySendAsync(
        Func<Task<EmailSendResult>> send,
        string notificationName,
        Guid workItemId,
        CancellationToken cancellationToken)
    {
        try
        {
            return await send();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to send {NotificationName} email for work item {WorkItemId}. The transaction was already committed.",
                notificationName,
                workItemId);

            return new EmailSendResult
            {
                Success = false,
                ErrorCode = ex.GetType().Name,
                ErrorMessage = "Email notification failed."
            };
        }
    }
}
