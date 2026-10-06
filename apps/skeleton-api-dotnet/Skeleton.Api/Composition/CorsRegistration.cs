namespace Skeleton.Api.Composition;

public static class CorsRegistration
{
    public const string PolicyName = "SkeletonCors";

    public static IServiceCollection AddAppCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSection = configuration.GetRequiredSection(CorsConfiguration.SectionName);
        var corsConfiguration = corsSection.Get<CorsConfiguration>() ?? new CorsConfiguration();
        var allowedOrigins = corsConfiguration.AllowedOrigins.Values
            .Select(NormalizeOrigin)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        services
            .AddOptions<CorsConfiguration>()
            .Bind(corsSection)
            .Validate(options => options.AllowedOrigins.Count > 0, "Cors:AllowedOrigins must contain at least one origin.")
            .Validate(
                options => options.AllowedOrigins.Values.All(IsValidOrigin),
                "Cors:AllowedOrigins values must be absolute HTTP or HTTPS origins without paths, query strings, or fragments.")
            .ValidateOnStart();

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    private static string NormalizeOrigin(string origin)
    {
        return TryNormalizeOrigin(origin, out var normalizedOrigin)
            ? normalizedOrigin
            : origin;
    }

    private static bool IsValidOrigin(string origin) =>
        TryNormalizeOrigin(origin, out _);

    private static bool TryNormalizeOrigin(string? origin, out string normalizedOrigin)
    {
        normalizedOrigin = string.Empty;

        if (string.IsNullOrWhiteSpace(origin))
        {
            return false;
        }

        if (!Uri.TryCreate(origin.Trim(), UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (!string.Equals(uri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(uri.Query)
            || !string.IsNullOrEmpty(uri.Fragment)
            || uri.AbsolutePath is not "" and not "/")
        {
            return false;
        }

        normalizedOrigin = uri.GetLeftPart(UriPartial.Authority);
        return true;
    }
}

internal sealed class CorsConfiguration
{
    public const string SectionName = "Cors";

    public Dictionary<string, string> AllowedOrigins { get; init; } = [];
}
