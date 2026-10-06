using Azure;
using Azure.Communication.Email;
using Skeleton.Core.Infrastructure.Communications.Email.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EmailSendResult = Skeleton.Core.Infrastructure.Communications.Email.Models.EmailSendResult;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed class AzureCommunicationEmailSender(
    EmailClient emailClient,
    IOptions<EmailOptions> options,
    ILogger<AzureCommunicationEmailSender> logger)
    : IEmailSender
{
    private readonly EmailClient _emailClient = emailClient;
    private readonly ILogger<AzureCommunicationEmailSender> _logger = logger;
    private readonly string _fromAddress = options.Value.FromAddress;

    public async Task<EmailSendResult> SendAsync(
        EmailMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var message = BuildMessage(request);

            var operation = await _emailClient.SendAsync(
                WaitUntil.Completed,
                message,
                cancellationToken);

            _logger.LogInformation(
                "Email sent to {To}. OperationId: {OperationId}. Status: {Status}",
                request.To,
                operation.Id,
                operation.Value.Status);

            return new EmailSendResult
            {
                Success = true,
                OperationId = operation.Id,
                Status = operation.Value.Status.ToString()
            };
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(
                ex,
                "Failed to send email to {To}. ErrorCode: {ErrorCode}",
                request.To,
                ex.ErrorCode);

            return new EmailSendResult
            {
                Success = false,
                ErrorCode = ex.ErrorCode,
                ErrorMessage = ex.Message
            };
        }
    }

    private EmailMessage BuildMessage(EmailMessageRequest request)
    {
        var content = new EmailContent(request.Subject)
        {
            Html = request.HtmlBody
        };

        var message = new EmailMessage(
            _fromAddress,
            request.To,
            content);

        foreach (var attachment in request.Attachments)
        {
            message.Attachments.Add(
                new EmailAttachment(
                    attachment.FileName,
                    attachment.ContentType,
                    BinaryData.FromBytes(attachment.Content)));
        }

        return message;
    }
}
