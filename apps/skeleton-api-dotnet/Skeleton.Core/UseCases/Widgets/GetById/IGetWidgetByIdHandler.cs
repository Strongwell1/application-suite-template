namespace Skeleton.Core.UseCases.Widgets.GetById;

public interface IGetWidgetByIdHandler
{
    Task<GetWidgetByIdResult?> Handle(
        GetWidgetByIdQuery query,
        CancellationToken cancellationToken);
}
