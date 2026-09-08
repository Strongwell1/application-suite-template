namespace Skeleton.Core.UseCases.Widgets.List;

public sealed class ListWidgetsHandler : IListWidgetsHandler
{
    public Task<ListWidgetsResult> Handle(
        ListWidgetsQuery query,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
