using System.Globalization;

namespace TestableHttpClient;

internal sealed record UriPattern
{
    public UriPattern()
    {
        Scheme = new Any();
        Host = new Any();
        Port = new Any();
        Path = new Any();
        Query = new Any();
    }

    public UriPatternValue Scheme { get; init; }
    public UriPatternValue Host { get; init; }
    public UriPatternValue Port { get; init; }
    public UriPatternValue Path { get; init; }
    public UriPatternValue Query { get; init; }

    public bool Matches(Uri requestUri, UriPatternMatchingOptions options) =>
        Matches(Scheme, requestUri.Scheme, options.SchemeCaseInsensitive) &&
        Matches(Host, requestUri.Host, options.HostCaseInsensitive) &&
        Matches(Port, requestUri.Port.ToString(CultureInfo.InvariantCulture), true) &&
        Matches(Path, requestUri.AbsolutePath, options.PathCaseInsensitive) &&
        Matches(Query, requestUri.GetComponents(UriComponents.Query, options.DefaultQueryFormat), options.QueryCaseInsensitive);

    private static bool Matches(UriPatternValue patternValue, string value, bool ignoreCase) => patternValue switch
    {
        Any => true,
        ExactValue exactValue => exactValue.value.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal),
        PatternValue expectedPattern => StringMatcher.Matches(value, expectedPattern.pattern, ignoreCase)
    };
}
