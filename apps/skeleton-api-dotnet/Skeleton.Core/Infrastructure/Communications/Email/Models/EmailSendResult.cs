namespace Skeleton.Core.Infrastructure.Communications.Email.Models;

public sealed record EmailSendResult
{
    public required bool Success { get; init; }
    public string? OperationId { get; init; }
    public string? Status { get; init; }
    public string? ErrorCode { get; init; }
    public string? ErrorMessage { get; init; }
}
