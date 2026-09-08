using Azure.Identity;
using Azure.Storage.Blobs;
using Skeleton.Api.Infrastructure.Storage;
using Skeleton.Core.Infrastructure.Storage;

namespace Skeleton.Api.Composition;

public static class BlobStorageRegistration
{
    public static IServiceCollection AddBlobStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection("AzureStorage")
            .Get<AzureStorageOptions>()
            ?? throw new InvalidOperationException("AzureStorage configuration section is missing.");

        var credentialOptions = string.IsNullOrWhiteSpace(options.TenantId)
            ? null
            : new DefaultAzureCredentialOptions { TenantId = options.TenantId };

        services.AddSingleton(new BlobServiceClient(
            new Uri(options.AccountUrl),
            new DefaultAzureCredential(credentialOptions)));
        services.AddSingleton<IBlobStorageService>(sp =>
            new BlobStorageService(sp.GetRequiredService<BlobServiceClient>(), options.ContainerName));

        return services;
    }
}
