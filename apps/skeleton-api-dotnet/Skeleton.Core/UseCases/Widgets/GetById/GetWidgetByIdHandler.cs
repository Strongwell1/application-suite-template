namespace Skeleton.Core.UseCases.Widgets.GetById;

public sealed class GetWidgetByIdHandler : IGetWidgetByIdHandler
{
    public Task<GetWidgetByIdResult?> Handle(
        GetWidgetByIdQuery query,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
