using Serilog;

namespace Skeleton.Api.Composition;

public static class ApplicationPipeline
{
    public static WebApplication UseApplicationPipeline(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseSerilogRequestLogging();

        if (!app.Environment.IsAppDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseCors(CorsRegistration.PolicyName);

        // Authentication before rate limiting so the limiter can partition by
        // authenticated user (see RateLimitingRegistration) rather than only by IP.
        app.UseAuthentication();
        app.UseRateLimiter();
        app.UseAuthorization();

        app.MapAppHealthChecks();

        if (!app.Environment.IsDevelopmentOrDev()) return app;
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Skeleton API v1");
            options.RoutePrefix = "api/docs";
        });

        return app;
    }
}
