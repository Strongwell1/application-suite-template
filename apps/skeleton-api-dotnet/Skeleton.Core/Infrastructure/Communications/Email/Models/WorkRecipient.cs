namespace Skeleton.Core.Infrastructure.Communications.Email.Models;

public sealed record WorkRecipient
{
    public required string EmailAddress { get; init; }
    public required string RoleCode { get; init; }
}
