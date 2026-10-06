using Microsoft.AspNetCore.Mvc;
using Skeleton.Api.Security;
using Skeleton.Core.Contracts;
using Skeleton.Core.UseCases.Widgets.Delete;

namespace Skeleton.Api.Endpoints.Widgets.Delete;

public static class DeleteWidgetEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/widgets/{id:guid}", HandleAsync)
            .RequireAuthorization(Policies.RequireAdministratorAccess);
    }

    private static Task<IResult> HandleAsync(
        [FromServices] IDeleteWidgetHandler useCase,
        [FromServices] ICurrentUser currentUser,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
