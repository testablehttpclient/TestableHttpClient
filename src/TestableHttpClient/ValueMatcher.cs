using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TestableHttpClient;

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

    public static bool Matches(Request expected, HttpRequestMessage actual, UriPatternMatchingOptions options) =>
            Matches(expected.Method, actual.Method) &&
            Matches(expected.RequestUri, actual.RequestUri, options) &&
            Matches(expected.Version, actual.Version) &&
            Matches(expected.Headers, actual.GetHeaders()) &&
            Matches(expected.Content, actual.GetStringContent());

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

    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "https://github.com/dotnet/sdk/issues/57085")]
    internal static bool Matches(Headers expected, Dictionary<string, string> actual)
    {
        return expected switch
        {
            None => actual.Count == 0,
            Any => true,
            HeaderList headerList => headerList.Any(x => actual.TryGetValue(x.Key, out string? value) && Matches(x.Value, value))
        };
    }

    internal static bool Matches(HeaderValue expected, string actual)
    {
        return expected switch
        {
            Any => true,
            PatternValue pattern => Matches(pattern, actual, false)
        };
    }

    internal static bool Matches(Content expected, string? actual)
    {
        return expected switch
        {
            None => string.IsNullOrEmpty(actual),
            Any => true,
            PatternValue when actual is null => false,
            PatternValue pattern => Matches(pattern, actual, false),
        };
    }
}
