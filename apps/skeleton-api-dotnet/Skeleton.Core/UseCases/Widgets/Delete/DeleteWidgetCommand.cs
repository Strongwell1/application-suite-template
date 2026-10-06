using Skeleton.Core.Contracts;

namespace Skeleton.Core.UseCases.Widgets.Delete;

public sealed record DeleteWidgetCommand
{
    public required Guid Id { get; init; }
    public string? Reason { get; init; }
    public required ICurrentUser DeletedBy { get; init; }
}
