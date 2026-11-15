using System.Globalization;

namespace TestableHttpClient.Matchers;

internal static class UriPatternMatcher
{
    public static bool Matches(UriPattern expected, Uri actual, UriPatternMatchingOptions options)
    {
        return Matches(expected.Scheme, actual.Scheme, options.SchemeCaseInsensitive) &&
        Matches(expected.Host, actual.Host, options.HostCaseInsensitive) &&
        Matches(expected.Port, actual.Port.ToString(CultureInfo.InvariantCulture), true) &&
        Matches(expected.Path, actual.AbsolutePath, options.PathCaseInsensitive) &&
        Matches(expected.Query, actual.GetComponents(UriComponents.Query, options.DefaultQueryFormat), options.QueryCaseInsensitive);
    }

    private static bool Matches(UriPatternValue patternValue, string value, bool ignoreCase) => patternValue switch
    {
        Any => true,
        ExactValue exactValue => ValueMatcher.Matches(value, exactValue, ignoreCase),
        PatternValue expectedPattern => ValueMatcher.Matches(value, expectedPattern, ignoreCase)
    };
}
