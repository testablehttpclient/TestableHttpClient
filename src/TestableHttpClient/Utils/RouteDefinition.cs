namespace TestableHttpClient.Utils;

internal class RouteDefinition
{
    public static RouteDefinition Any { get; } = new RouteDefinition();

    public Value Scheme { get; init; } = Value.Any();
    public Value Host { get; init; } = Value.Any();
    public Value Path { get; init; } = Value.Any();

    public bool Matches(Uri requestUri) =>
        Scheme.Matches(requestUri.Scheme, true) &&
        Host.Matches(requestUri.Host, true) &&
        Path.Matches(requestUri.AbsolutePath, false);
}
