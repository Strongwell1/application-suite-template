using Microsoft.AspNetCore.Mvc;
using Skeleton.Api.Security;
using Skeleton.Core.UseCases.Shared.IdGeneration;
using Skeleton.Core.UseCases.Widgets.Attachments.Upload;

namespace Skeleton.Api.Endpoints.Widgets.Attachments;

// Example convention for file intake: size limit, extension + content-type allow-lists,
// DisableAntiforgery() for multipart posts, streamed straight into the use-case.
public static class UploadWidgetAttachmentEndpoint
{
    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        ".txt", ".md", ".csv", ".zip", ".jpg", ".jpeg", ".png"
    };

    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/vnd.ms-powerpoint",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "text/plain",
        "text/markdown",
        "text/csv",
        "application/zip",
        "image/jpeg",
        "image/png"
    };

    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/widgets/{id:guid}/attachments", HandleAsync)
            .RequireAuthorization(Policies.RequireUserAccess)
            .DisableAntiforgery();
    }

    private static async Task<IResult> HandleAsync(
        [FromServices] IUploadWidgetAttachmentHandler useCase,
        [FromServices] IIdGenerator idGenerator,
        [FromRoute] Guid id,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file.Length == 0)
            return Results.BadRequest("File is empty.");

        if (file.Length > MaxFileSizeBytes)
            return Results.BadRequest("File exceeds the 10 MB size limit.");

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(extension))
            return Results.BadRequest("File type is not allowed.");

        if (!AllowedContentTypes.Contains(file.ContentType))
            return Results.BadRequest("Content type is not allowed.");

        await using var stream = file.OpenReadStream();

        var result = await useCase.Handle(new UploadWidgetAttachmentCommand
        {
            WidgetId = id,
            AttachmentId = idGenerator.New(),
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileSizeBytes = file.Length,
            Content = stream,
        }, cancellationToken);

        return result switch
        {
            UploadWidgetAttachmentResult.Uploaded => Results.NoContent(),
            UploadWidgetAttachmentResult.WidgetNotFound => Results.NotFound(),
            _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
