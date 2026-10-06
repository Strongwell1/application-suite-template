namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed record WorkAssignedEmail
{
    public required Guid WorkItemId { get; init; }
}
