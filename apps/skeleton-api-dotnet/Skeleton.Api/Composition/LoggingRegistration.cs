using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace Skeleton.Api.Composition;

// Called directly from Program.cs via builder.Host.UseSerilog(...), not through
// CompositionRoot.AddDependencies — Serilog has to be wired up before the rest of the
// host is built, using HostBuilderContext rather than an already-built IServiceCollection.
public static class LoggingRegistration
{
    public static void ConfigureAppLogging(
        HostBuilderContext context,
        IServiceProvider services,
        LoggerConfiguration loggerConfiguration)
    {
        var environment = context.HostingEnvironment;

        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Environment", environment.EnvironmentName);

        // Human-readable in Development/Dev; compact JSON everywhere else so logs are
        // easy to ship to a log-aggregation backend later.
        if (environment.IsDevelopmentOrDev())
        {
            loggerConfiguration.WriteTo.Console();
        }
        else
        {
            loggerConfiguration.WriteTo.Console(new CompactJsonFormatter());
        }
    }
}
