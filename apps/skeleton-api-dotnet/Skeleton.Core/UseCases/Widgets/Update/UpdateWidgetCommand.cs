namespace Skeleton.Core.UseCases.Widgets.Update;

public sealed record UpdateWidgetCommand
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
