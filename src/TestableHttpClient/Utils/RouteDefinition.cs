namespace TestableHttpClient.Utils;

internal class RouteDefinition
{
    public static RouteDefinition Any { get; } = new RouteDefinition();

    public Value Scheme { get; init; } = Value.Any();
    public Value Host { get; init; } = Value.Any();
    public Value Path { get; init; } = Value.Any();

    public bool Matches(Uri requestUri, RoutingOptions routingOptions) =>
        Scheme.Matches(requestUri.Scheme, routingOptions.SchemeCaseInsensitive) &&
        Host.Matches(requestUri.Host, routingOptions.HostCaseInsensitive) &&
        Path.Matches(requestUri.AbsolutePath, routingOptions.PathCaseInsensitive);
}
