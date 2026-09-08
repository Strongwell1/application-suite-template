namespace Skeleton.Api.Endpoints.Widgets.List;

public sealed record ListWidgetsResponseItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required DateTime CreatedUtc { get; init; }
    public required Guid CreatedById { get; init; }
    public required string CreatedByDisplayName { get; init; }
    public DateTime? UpdatedUtc { get; init; }
}

public sealed record ListWidgetsResponse
{
    public required IReadOnlyList<ListWidgetsResponseItem> Items { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
}
