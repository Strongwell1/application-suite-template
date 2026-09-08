using Skeleton.Core.Infrastructure.Communications.Email.Models;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public interface IWorkRecipientService
{
    Task<WorkRecipient> ResolveAssignedRecipientAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default);
}
