using Microsoft.AspNetCore.Mvc;
using Skeleton.Api.Security;
using Skeleton.Core.Contracts;
using Skeleton.Core.UseCases.Widgets.Update;

namespace Skeleton.Api.Endpoints.Widgets.Update;

public static class UpdateWidgetEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/widgets/{id:guid}", HandleAsync)
            .RequireAuthorization(Policies.RequireAdministratorAccess);
    }

    private static Task<IResult> HandleAsync(
        [FromServices] IUpdateWidgetHandler useCase,
        [FromServices] ICurrentUser currentUser,
        [FromRoute] Guid id,
        [FromBody] UpdateWidgetRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
