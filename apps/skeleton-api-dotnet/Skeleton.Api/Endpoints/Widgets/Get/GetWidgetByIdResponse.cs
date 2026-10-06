namespace Skeleton.Api.Endpoints.Widgets.Get;

public sealed record GetWidgetByIdResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required DateTime CreatedUtc { get; init; }
    public required Guid CreatedById { get; init; }
    public required string CreatedByDisplayName { get; init; }
    public DateTime? UpdatedUtc { get; init; }
}
