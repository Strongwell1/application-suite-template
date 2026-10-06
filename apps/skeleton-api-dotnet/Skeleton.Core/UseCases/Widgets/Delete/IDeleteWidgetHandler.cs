namespace Skeleton.Core.UseCases.Widgets.Delete;

public interface IDeleteWidgetHandler
{
    Task<DeleteWidgetResult> Handle(
        DeleteWidgetCommand command,
        CancellationToken cancellationToken);
}
