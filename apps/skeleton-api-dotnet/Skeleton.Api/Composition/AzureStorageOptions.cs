namespace Skeleton.Api.Composition;

public sealed class AzureStorageOptions
{
    public required string AccountUrl { get; init; }
    public required string ContainerName { get; init; }
    public string? TenantId { get; init; }
}
