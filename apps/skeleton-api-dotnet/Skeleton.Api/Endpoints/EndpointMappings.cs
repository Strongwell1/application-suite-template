using Skeleton.Api.Endpoints.Widgets;

namespace Skeleton.Api.Endpoints;

public static class EndpointMappings
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWidgetEndpoints();
    }
}
