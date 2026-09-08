namespace Skeleton.Core.UseCases.Widgets.List;

public sealed record ListWidgetsQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 25;
}
