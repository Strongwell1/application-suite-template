using Skeleton.Core.Infrastructure.Communications.Email.Models;
using Skeleton.Core.Infrastructure.Communications.Email.Templates;
using Skeleton.Core.Persistence;
using Microsoft.Extensions.Configuration;

namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed class WorkEmailService(
    IEmailService emailService,
    IWorkRecipientService workRecipientService,
    AppDbContext dbContext,
    IConfiguration configuration)
    : IWorkEmailService
{
    private readonly AppDbContext _dbContext = dbContext;
    private const string TasksPath = "/tasks";

    public async Task<EmailSendResult> SendWorkAssignedAsync(
        WorkAssignedEmail email,
        CancellationToken cancellationToken = default)
    {
        var recipient = await workRecipientService.ResolveAssignedRecipientAsync(
            email.WorkItemId,
            cancellationToken);

        var baseUrl = ReadSpaBaseUrl();
        var taskUrl = $"{baseUrl}{TasksPath}";

        var template = WorkAssignedTemplate.Render(
            "Work Assigned",
            "Work Assigned",
            recipient.RoleCode,
            WorkAssignedTemplate.Paragraph("You have been assigned new work."),
            taskUrl,
            "View Tasks");

        return await emailService.SendAsync(
            recipient.EmailAddress,
            "Work Assigned",
            template,
            new Dictionary<string, string>(),
            cancellationToken: cancellationToken);
    }

    public Task<EmailSendResult> SendWorkCompletedAsync(
        WorkCompletedEmail email,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private string ReadSpaBaseUrl()
    {
        var spaBaseUrl = configuration["App:SpaBaseUrl"]?.TrimEnd('/');

        if (string.IsNullOrWhiteSpace(spaBaseUrl))
        {
            throw new InvalidOperationException("Missing configuration value: App:SpaBaseUrl");
        }

        return spaBaseUrl;
    }
}
