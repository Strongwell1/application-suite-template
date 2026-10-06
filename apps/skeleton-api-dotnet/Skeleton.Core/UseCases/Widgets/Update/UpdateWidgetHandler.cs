namespace Skeleton.Core.UseCases.Widgets.Update;

public sealed class UpdateWidgetHandler : IUpdateWidgetHandler
{
    public Task<UpdateWidgetResult> Handle(
        UpdateWidgetCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
