namespace Skeleton.Core.Infrastructure.Communications.Email.Models;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    public string FromAddress { get; init; } = string.Empty;
    public string ConnectionString { get; init; } = string.Empty;
    public required string OverrideToAddress { get; init; }
    public required string EnvironmentName { get; set; }
}
