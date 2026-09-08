using Skeleton.Core.Contracts;

namespace Skeleton.Core.UseCases.Widgets.Create;

public sealed record CreateWidgetCommand
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required ICurrentUser CreatedBy { get; init; }
}
