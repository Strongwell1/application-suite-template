using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Skeleton.Api.Security;

namespace Skeleton.Api.Composition;

public static class RateLimitingRegistration
{
    public static IServiceCollection AddAppRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection(RateLimitingConfiguration.SectionName);

        services
            .AddOptions<RateLimitingConfiguration>()
            .Bind(section)
            .Validate(options => options.PermitLimit > 0, "RateLimiting:PermitLimit must be greater than zero.")
            .Validate(options => options.WindowSeconds > 0, "RateLimiting:WindowSeconds must be greater than zero.")
            .Validate(options => options.QueueLimit >= 0, "RateLimiting:QueueLimit cannot be negative.")
            .ValidateOnStart();

        var rateLimitingConfiguration = section.Get<RateLimitingConfiguration>() ?? new RateLimitingConfiguration();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Partitioned by caller (authenticated user, falling back to IP for anonymous
            // requests) rather than one shared bucket — one noisy client shouldn't be able
            // to exhaust everyone else's quota.
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var partitionKey = httpContext.User.FindFirst(ClaimsCurrentUser.ObjectIdClaimType)?.Value
                    ?? httpContext.Connection.RemoteIpAddress?.ToString()
                    ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = rateLimitingConfiguration.PermitLimit,
                    Window = TimeSpan.FromSeconds(rateLimitingConfiguration.WindowSeconds),
                    QueueLimit = rateLimitingConfiguration.QueueLimit,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                });
            });
        });

        return services;
    }
}

internal sealed class RateLimitingConfiguration
{
    public const string SectionName = "RateLimiting";

    public int PermitLimit { get; init; } = 100;
    public int WindowSeconds { get; init; } = 60;
    public int QueueLimit { get; init; }
}
