using System.Globalization;

namespace TestableHttpClient;

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
        ExactValue exactValue => exactValue.value.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal),
        PatternValue expectedPattern => StringMatcher.Matches(value, expectedPattern.pattern, ignoreCase)
    };
}
