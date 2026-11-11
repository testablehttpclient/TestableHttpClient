using System.Globalization;

namespace TestableHttpClient.Utils;

internal sealed record UriPattern
{
    public static UriPattern Any { get; } = new UriPattern();

    public UriPatternValue Scheme { get; init; } = UriPatternValue.CreateAny();
    public UriPatternValue Host { get; init; } = UriPatternValue.CreateAny();
    public UriPatternValue Port { get; init; } = UriPatternValue.CreateAny();
    public UriPatternValue Path { get; init; } = UriPatternValue.CreateAny();
    public UriPatternValue Query { get; init; } = UriPatternValue.CreateAny();

    public bool Matches(Uri requestUri, UriPatternMatchingOptions options) =>
        Scheme.Matches(requestUri.Scheme, options.SchemeCaseInsensitive) &&
        Host.Matches(requestUri.Host, options.HostCaseInsensitive) &&
        Port.Matches(requestUri.Port.ToString(CultureInfo.InvariantCulture), true) &&
        Path.Matches(requestUri.AbsolutePath, options.PathCaseInsensitive) &&
        Query.Matches(requestUri.GetComponents(UriComponents.Query, options.DefaultQueryFormat), options.QueryCaseInsensitive);
}

