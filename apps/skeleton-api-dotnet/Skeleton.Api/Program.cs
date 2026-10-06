using Microsoft.Extensions.Hosting;
using Serilog;
using Skeleton.Api.Composition;
using Skeleton.Api.Endpoints;

// Bootstrap logger: catches anything that goes wrong before the full logging pipeline
// (configured from IConfiguration/DI below) is available.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(LoggingRegistration.ConfigureAppLogging);

    builder.Services.AddDependencies(builder.Configuration, builder.Environment);

    var app = builder.Build();

    app.UseApplicationPipeline();

    app.MapEndpoints();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    // HostAbortedException is thrown on purpose by `dotnet ef` design-time tooling
    // (it builds the host just far enough to get a DbContext, then aborts) — not a
    // real startup failure, so it shouldn't be logged as one.
    Log.Fatal(ex, "Application terminated unexpectedly");
    Environment.ExitCode = 1;
}
finally
{
    Log.CloseAndFlush();
}
