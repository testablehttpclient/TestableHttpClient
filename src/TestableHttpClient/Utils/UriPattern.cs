namespace TestableHttpClient.Utils;

internal class UriPattern
{
    public static UriPattern Any { get; } = new UriPattern();

    public Value Scheme { get; init; } = Value.Any();
    public Value Host { get; init; } = Value.Any();
    public Value Path { get; init; } = Value.Any();

    public bool Matches(Uri requestUri, RoutingOptions routingOptions) =>
        Scheme.Matches(requestUri.Scheme, routingOptions.SchemeCaseInsensitive) &&
        Host.Matches(requestUri.Host, routingOptions.HostCaseInsensitive) &&
        Path.Matches(requestUri.AbsolutePath, routingOptions.PathCaseInsensitive);
}

