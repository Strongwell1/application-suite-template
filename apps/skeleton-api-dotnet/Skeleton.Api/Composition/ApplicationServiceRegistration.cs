using Skeleton.Api.ErrorHandling;
using Skeleton.Api.Security;
using Skeleton.Core.Contracts;
using Skeleton.Core.Infrastructure.Communications.Email;
using Skeleton.Core.UseCases.Shared.IdGeneration;
using Skeleton.Core.UseCases.Widgets.Services;

namespace Skeleton.Api.Composition;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
    {
        // ASP.NET Core infrastructure
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Request/user context
        services.AddScoped<ICurrentUser, ClaimsCurrentUser>();

        // Application services
        services.AddSingleton<IIdGenerator, SequentialIdGenerator>();
        services.AddScoped<IWidgetService, WidgetService>();
        services.AddScoped<IWorkRecipientService, WorkRecipientService>();

        return services;
    }
}
