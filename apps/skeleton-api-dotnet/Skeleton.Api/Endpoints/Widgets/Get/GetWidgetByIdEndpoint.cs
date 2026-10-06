using Microsoft.AspNetCore.Mvc;
using Skeleton.Api.Security;
using Skeleton.Core.Contracts;
using Skeleton.Core.UseCases.Widgets.GetById;

namespace Skeleton.Api.Endpoints.Widgets.Get;

public static class GetWidgetByIdEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/widgets/{id:guid}", HandleAsync)
            .RequireAuthorization(Policies.RequireUserAccess);
    }

    private static Task<IResult> HandleAsync(
        [FromServices] IGetWidgetByIdHandler useCase,
        [FromServices] ICurrentUser currentUser,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
