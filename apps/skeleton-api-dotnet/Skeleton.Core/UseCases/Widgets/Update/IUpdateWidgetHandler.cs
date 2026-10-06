namespace Skeleton.Core.UseCases.Widgets.Update;

public interface IUpdateWidgetHandler
{
    Task<UpdateWidgetResult> Handle(
        UpdateWidgetCommand command,
        CancellationToken cancellationToken);
}
