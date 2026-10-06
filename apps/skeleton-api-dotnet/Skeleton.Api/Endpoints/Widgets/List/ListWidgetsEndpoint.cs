using Microsoft.AspNetCore.Mvc;
using Skeleton.Api.Security;
using Skeleton.Core.Contracts;
using Skeleton.Core.UseCases.Widgets.List;

namespace Skeleton.Api.Endpoints.Widgets.List;

public static class ListWidgetsEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/widgets", HandleAsync)
            .RequireAuthorization(Policies.RequireUserAccess);
    }

    private static Task<IResult> HandleAsync(
        [FromServices] IListWidgetsHandler useCase,
        [FromServices] ICurrentUser currentUser,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
