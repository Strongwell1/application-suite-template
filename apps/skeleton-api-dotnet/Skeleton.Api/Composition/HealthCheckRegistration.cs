using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Skeleton.Core.Persistence;

namespace Skeleton.Api.Composition;

public static class HealthCheckRegistration
{
    private const string ReadyTag = "ready";

    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("database", tags: [ReadyTag]);

        return services;
    }

    public static IEndpointRouteBuilder MapAppHealthChecks(this IEndpointRouteBuilder app)
    {
        // Liveness: is the process itself up? No dependency checks — used by an
        // orchestrator to decide whether to restart the instance.
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false,
        }).DisableRateLimiting();

        // Readiness: can this instance actually serve traffic right now? Checks
        // dependencies (currently just the database) tagged "ready".
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains(ReadyTag),
        }).DisableRateLimiting();

        return app;
    }
}
