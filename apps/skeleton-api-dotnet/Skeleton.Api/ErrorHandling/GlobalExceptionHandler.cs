using Skeleton.Core.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace Skeleton.Api.ErrorHandling;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case IBusinessRuleException businessRule:
            {
                logger.LogWarning(exception, "Business rule violation: {Field}", businessRule.Field);

                var result = ApiProblemDetails.UnprocessableEntity(businessRule.Field, exception.Message);
                await result.ExecuteAsync(httpContext);
                return true;
            }

            case IConflictException conflict:
            {
                logger.LogWarning(exception, "Conflict on {Field}", conflict.Field);

                var result = ApiProblemDetails.Conflict(conflict.Field, exception.Message);
                await result.ExecuteAsync(httpContext);
                return true;
            }

            case ArgumentException argEx:
            {
                logger.LogWarning(exception, "Validation error on {ParamName}", argEx.ParamName);

                var result = ApiProblemDetails.Validation(argEx.ParamName ?? "request", argEx.Message);
                await result.ExecuteAsync(httpContext);
                return true;
            }
        }

        logger.LogError(exception, "Unhandled exception");

        var problem = Results.Problem(
            title: "Internal Server Error",
            statusCode: StatusCodes.Status500InternalServerError);

        await problem.ExecuteAsync(httpContext);
        return true;
    }
}
