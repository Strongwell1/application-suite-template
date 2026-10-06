using Skeleton.Api.Endpoints.Widgets.Attachments;
using Skeleton.Api.Endpoints.Widgets.Create.Standard;
using Skeleton.Api.Endpoints.Widgets.Delete;
using Skeleton.Api.Endpoints.Widgets.Get;
using Skeleton.Api.Endpoints.Widgets.List;
using Skeleton.Api.Endpoints.Widgets.Update;

namespace Skeleton.Api.Endpoints.Widgets;

public static class WidgetEndpointMappings
{
    public static void MapWidgetEndpoints(this IEndpointRouteBuilder app)
    {
        CreateWidgetEndpoint.Map(app);
        GetWidgetByIdEndpoint.Map(app);
        ListWidgetsEndpoint.Map(app);
        UpdateWidgetEndpoint.Map(app);
        DeleteWidgetEndpoint.Map(app);
        UploadWidgetAttachmentEndpoint.Map(app);
        GetWidgetAttachmentEndpoint.Map(app);
    }
}
