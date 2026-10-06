namespace Skeleton.Core.Infrastructure.Communications.Email.Models;

public sealed record EmailMessageRequest
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string HtmlBody { get; init; }
    public IReadOnlyList<EmailAttachmentRequest> Attachments { get; init; } = [];
}
