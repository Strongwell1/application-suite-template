using Skeleton.Core.Persistence;
using Skeleton.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Skeleton.Api.Composition;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("efConnectionStrings.json", optional: false)
            .Build();

        var environment = configuration["Environment"]
                          ?? throw new InvalidOperationException("Environment was not found in efConnectionStrings.json.");

        if (!ApplicationEnvironmentNames.IsSupported(environment))
        {
            throw new InvalidOperationException(
                $"Environment '{environment}' is not supported. Supported values: {string.Join(", ", ApplicationEnvironmentNames.Supported)}.");
        }

        var connectionString = configuration.GetSection("ConnectionStrings")[environment]
                               ?? throw new InvalidOperationException(
                                   $"Connection string for environment '{environment}' was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
