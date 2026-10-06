namespace Skeleton.Api.Endpoints.Widgets.Create.Standard;

public sealed record CreateWidgetRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}
