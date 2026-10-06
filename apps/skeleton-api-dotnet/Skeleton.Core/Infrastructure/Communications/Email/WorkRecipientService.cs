using Skeleton.Core.Infrastructure.Communications.Email.Models;
using Skeleton.Core.Persistence;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed class WorkRecipientService(AppDbContext dbContext) : IWorkRecipientService
{
    private readonly AppDbContext _dbContext = dbContext;
    public Task<WorkRecipient> ResolveAssignedRecipientAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
