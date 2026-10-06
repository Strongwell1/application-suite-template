using Microsoft.AspNetCore.Mvc;

namespace Skeleton.Api.ErrorHandling;

public static class ApiProblemDetails
{
    public static IResult Validation(string field, string message)
    {
        var details = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            [field] = [message]
        })
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred."
        };

        return Results.Json(details, statusCode: details.Status);
    }

    public static IResult UnprocessableEntity(string field, string message)
    {
        var details = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            [field] = [message]
        })
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "One or more validation errors occurred."
        };

        return Results.Json(details, statusCode: details.Status);
    }

    public static IResult Conflict(string field, string message)
    {
        var details = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            [field] = [message]
        })
        {
            Status = StatusCodes.Status409Conflict,
            Title = "The request could not be completed because of the current resource state."
        };

        return Results.Json(details, statusCode: details.Status);
    }
}
