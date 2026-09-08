using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Skeleton.Api.Security;
using Skeleton.Core.Infrastructure.Storage;
using Skeleton.Core.UseCases.Widgets.Attachments.Get;

namespace Skeleton.Api.Endpoints.Widgets.Attachments;

// Example convention for file delivery: inline vs. attachment disposition based on a
// content-type allow-list, streamed straight from blob storage via Results.Stream.
public static class GetWidgetAttachmentEndpoint
{
    private static readonly HashSet<string> InlineContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf", "image/jpeg", "image/png"
    };

    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/widgets/{id:guid}/attachments/{attachmentId:guid}", HandleAsync)
            .RequireAuthorization(Policies.RequireUserAccess);
    }

    private static Task<IResult> HandleAsync(
        [FromServices] IGetWidgetAttachmentHandler useCase,
        [FromServices] IBlobStorageService blobStorage,
        HttpContext httpContext,
        [FromRoute] Guid id,
        [FromRoute] Guid attachmentId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
