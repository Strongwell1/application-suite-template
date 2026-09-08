namespace Skeleton.Core.UseCases.Widgets.List;

public interface IListWidgetsHandler
{
    Task<ListWidgetsResult> Handle(
        ListWidgetsQuery query,
        CancellationToken cancellationToken);
}
