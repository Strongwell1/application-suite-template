namespace Skeleton.Api.Endpoints.Widgets.Update;

public sealed record UpdateWidgetRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
