namespace Skeleton.Core.UseCases.Widgets.Create;

public sealed class CreateWidgetHandler : ICreateWidgetHandler
{
    public Task<CreateWidgetResult> Handle(
        CreateWidgetCommand command,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
