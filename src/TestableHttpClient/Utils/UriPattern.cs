using System.Globalization;

namespace TestableHttpClient.Utils;

internal sealed class UriPattern
{
    public static UriPattern Any { get; } = new UriPattern();

    public Value<string> Scheme { get; init; } = Value.Any();
    public Value<string> Host { get; init; } = Value.Any();
    public Value<string> Port { get; init; } = Value.Any();
    public Value<string> Path { get; init; } = Value.Any();
    public Value<string> Query { get; init; } = Value.Any();

    public bool Matches(Uri? requestUri, UriPatternMatchingOptions options) =>
        (requestUri is null && this == Any) ||
        requestUri is not null &&
        Scheme.Matches(requestUri.Scheme, options.SchemeCaseInsensitive) &&
        Host.Matches(requestUri.Host, options.HostCaseInsensitive) &&
        Port.Matches(requestUri.Port.ToString(CultureInfo.InvariantCulture), true) &&
        Path.Matches(requestUri.AbsolutePath, options.PathCaseInsensitive) &&
        Query.Matches(requestUri.GetComponents(UriComponents.Query, options.DefaultQueryFormat), options.QueryCaseInsensitive);
}

