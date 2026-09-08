namespace Skeleton.Core.UseCases.Widgets.Create;

public interface ICreateWidgetHandler
{
    Task<CreateWidgetResult> Handle(
        CreateWidgetCommand command,
        CancellationToken cancellationToken);
}
