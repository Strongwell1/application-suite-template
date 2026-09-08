namespace Skeleton.Api.Security;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.RequireDeveloperAccess, policy =>
                policy.RequireRole(Roles.Developer))
            .AddPolicy(Policies.RequireAdministratorAccess, policy =>
                policy.RequireRole(
                    Roles.Developer,
                    Roles.Administrator))
            .AddPolicy(Policies.RequireUserAccess, policy =>
                policy.RequireRole(
                    Roles.Developer,
                    Roles.Administrator,
                    Roles.User));

        return services;
    }
}
