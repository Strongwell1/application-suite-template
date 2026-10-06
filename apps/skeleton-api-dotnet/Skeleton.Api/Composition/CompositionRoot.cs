using Skeleton.Api.Security;
using Skeleton.Api.Security.OpenApi;

namespace Skeleton.Api.Composition;

public static class CompositionRoot
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddPersistenceServices(configuration);
        services.AddAppCors(configuration);
        services.AddAppAuthentication(configuration);
        services.AddAppAuthorization();
        services.AddAppRateLimiting(configuration);
        services.AddAppHealthChecks();
        services.AddServiceRegistration();
        services.AddUseCaseServices();
        services.AddEmailServices(configuration, environment);
        services.AddBlobStorage(configuration);

        services.AddOpenApi(options =>
        {
            if (environment.IsDevelopmentOrDev()) options.AddDocumentTransformer<BearerTransformer>();
        });

        return services;
    }
}
