using Skeleton.Core.Configuration;

namespace Skeleton.Api.Composition;

public static class EnvironmentExtensions
{
    extension(IHostEnvironment environment)
    {
        public bool IsAppDevelopment() =>
            environment.IsEnvironment(ApplicationEnvironmentNames.Development);

        public bool IsDev() =>
            environment.IsEnvironment(ApplicationEnvironmentNames.Dev);

        public bool IsUat() =>
            environment.IsEnvironment(ApplicationEnvironmentNames.Uat);

        public bool IsDevelopmentOrDev() =>
            environment.IsAppDevelopment() || environment.IsDev();
    }
}
