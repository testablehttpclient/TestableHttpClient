using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TestableHttpClient.Matchers;

internal static class ValueMatcher
{
    internal static bool Matches(ExactValue expected, string actual, bool ignoreCase) => expected.Value.Equals(actual, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

    internal static bool Matches(PatternValue expected, string actual, bool ignoreCase)
    {
        string escapedPattern = Regex.Escape(expected.Pattern);

        string regex = escapedPattern.Replace("\\*", "(.*)", StringComparison.Ordinal);
        RegexOptions options = RegexOptions.Singleline;
        if (ignoreCase)
        {
            options |= RegexOptions.IgnoreCase;
        }

        return Regex.IsMatch(actual, $"^{regex}\\z", options); // Using \z to match end of string.
    }

    public static bool Matches(UriPattern expected, Uri? actual, UriPatternMatchingOptions options)
    {
        if (actual is null)
        {
            return expected.IsAny;
        }

        return Matches(expected.Scheme, actual.Scheme, options.SchemeCaseInsensitive) &&
               Matches(expected.Host, actual.Host, options.HostCaseInsensitive) &&
               Matches(expected.Port, actual.Port.ToString(CultureInfo.InvariantCulture), true) &&
               Matches(expected.Path, actual.AbsolutePath, options.PathCaseInsensitive) &&
               Matches(expected.Query, actual.GetComponents(UriComponents.Query, options.DefaultQueryFormat), options.QueryCaseInsensitive);
    }

    private static bool Matches(UriPatternValue patternValue, string value, bool ignoreCase) => patternValue switch
    {
        Any => true,
        ExactValue exactValue => Matches(exactValue, value, ignoreCase),
        PatternValue expectedPattern => Matches(expectedPattern, value, ignoreCase)
    };

    public static bool Matches(Request expectedRequest, HttpRequestMessage actual, UriPatternMatchingOptions options) => RequestMatcher.Matches(expectedRequest, actual, options);

    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "https://github.com/dotnet/sdk/issues/57085")]
    internal static bool Matches(HttpMethodOrAny expected, HttpMethod actual) => expected switch
    {
        Any => true,
        HttpMethod httpMethod => httpMethod == actual
    };

    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "https://github.com/dotnet/sdk/issues/57085")]
    internal static bool Matches(VersionOrAny expected, Version actual) => expected switch
    {
        Any => true,
        Version httpVersion => httpVersion == actual
    };
}
