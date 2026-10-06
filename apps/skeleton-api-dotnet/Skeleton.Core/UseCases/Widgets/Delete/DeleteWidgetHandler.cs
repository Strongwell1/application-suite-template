namespace Skeleton.Core.UseCases.Widgets.Delete;

// Convention: soft delete. When implemented, this should load the widget, call
// Widget.Delete(deletedBy, reason, DateTime.UtcNow) and save — not remove the row.
public sealed class DeleteWidgetHandler : IDeleteWidgetHandler
{
    public Task<DeleteWidgetResult> Handle(
        DeleteWidgetCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
