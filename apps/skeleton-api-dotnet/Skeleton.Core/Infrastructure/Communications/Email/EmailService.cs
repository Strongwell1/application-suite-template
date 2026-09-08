using Skeleton.Core.Infrastructure.Communications.Email.Models;
using Skeleton.Core.Configuration;
using Microsoft.Extensions.Options;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed class EmailService(
    IEmailSender emailSender,
    IEmailTemplateRenderer templateRenderer,
    IOptions<EmailOptions> options)
    : IEmailService
{
    private readonly EmailOptions _options = options.Value;

    public async Task<EmailSendResult> SendAsync(
        string to,
        string subject,
        string template,
        IReadOnlyDictionary<string, string> templateValues,
        IReadOnlyList<EmailAttachmentRequest>? attachments = null,
        CancellationToken cancellationToken = default)
    {
        var htmlBody = templateRenderer.Render(template, templateValues);

        if (!string.Equals(_options.EnvironmentName, ApplicationEnvironmentNames.Prod, StringComparison.Ordinal))
        {
            htmlBody = $"""
                        <p style="color:#b00020;">
                            <strong>Non-production email override.</strong><br />
                            Original recipient: {to}
                        </p>
                        <hr />
                        {htmlBody}
                        """;

            to = _options.OverrideToAddress;
            subject = $"[{_options.EnvironmentName}] {subject}";
        }

        var request = new EmailMessageRequest
        {
            To = to,
            Subject = subject,
            HtmlBody = htmlBody,
            Attachments = attachments ?? []
        };

        return await emailSender.SendAsync(request, cancellationToken);
    }
}
