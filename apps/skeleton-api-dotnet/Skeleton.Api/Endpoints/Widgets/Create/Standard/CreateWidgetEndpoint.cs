using Microsoft.AspNetCore.Mvc;
using Skeleton.Api.Security;
using Skeleton.Core.Contracts;
using Skeleton.Core.UseCases.Widgets.Create;

namespace Skeleton.Api.Endpoints.Widgets.Create.Standard;

public static class CreateWidgetEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/widgets", HandleAsync)
            .RequireAuthorization(Policies.RequireAdministratorAccess);
    }

    private static Task<IResult> HandleAsync(
        [FromServices] ICreateWidgetHandler useCase,
        [FromServices] ICurrentUser currentUser,
        [FromBody] CreateWidgetRequest request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
