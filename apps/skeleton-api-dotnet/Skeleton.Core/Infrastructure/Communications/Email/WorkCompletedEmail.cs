namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed record WorkCompletedEmail
{
    public required Guid WorkItemId { get; init; }
}
