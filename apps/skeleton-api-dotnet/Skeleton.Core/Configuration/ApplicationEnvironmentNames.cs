namespace Skeleton.Core.Configuration;

public static class ApplicationEnvironmentNames
{
    public const string Development = "development";
    public const string Dev = "dev";
    public const string Uat = "uat";
    public const string Prod = "prod";

    public static IReadOnlyCollection<string> Supported { get; } =
        [Development, Dev, Uat, Prod];

    public static bool IsSupported(string environmentName) =>
        string.Equals(environmentName, Development, StringComparison.Ordinal)
        || string.Equals(environmentName, Dev, StringComparison.Ordinal)
        || string.Equals(environmentName, Uat, StringComparison.Ordinal)
        || string.Equals(environmentName, Prod, StringComparison.Ordinal);
}
