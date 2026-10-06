using Azure.Communication.Email;
using Skeleton.Core.Infrastructure.Communications.Email;
using Skeleton.Core.Infrastructure.Communications.Email.Models;
using Microsoft.Extensions.Options;

namespace Skeleton.Api.Composition;

public static class EmailRegistration
{
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services
            .AddOptions<EmailOptions>()
            .Bind(configuration.GetSection(EmailOptions.SectionName))
            .PostConfigure(options =>
            {
                options.EnvironmentName = environment.EnvironmentName;
            })
            .Validate(options => !string.IsNullOrWhiteSpace(options.FromAddress), "Email FromAddress is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString), "Email ConnectionString is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.OverrideToAddress), "Email OverrideToAddress is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.EnvironmentName), "Email EnvironmentName is required.")
            .ValidateOnStart();

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<EmailOptions>>().Value;

            return new EmailClient(options.ConnectionString);
        });

        services.AddScoped<IEmailSender, AzureCommunicationEmailSender>();
        services.AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<WorkEmailService>();
        services.AddScoped<Func<WorkEmailService>>(sp =>
            () => sp.GetRequiredService<WorkEmailService>());
        services.AddScoped<IWorkEmailService, SafeWorkEmailService>();

        return services;
    }
}
