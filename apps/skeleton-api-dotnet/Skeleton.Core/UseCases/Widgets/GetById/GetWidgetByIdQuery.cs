namespace Skeleton.Core.UseCases.Widgets.GetById;

public sealed record GetWidgetByIdQuery
{
    public required Guid Id { get; init; }
}
