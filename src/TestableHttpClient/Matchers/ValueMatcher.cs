using System.Text.RegularExpressions;

namespace TestableHttpClient.Matchers;

internal static class ValueMatcher
{
    internal static bool Matches(string value, ExactValue exactValue, bool ignoreCase) => exactValue.Value.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    internal static bool Matches(string value, PatternValue pattern, bool ignoreCase)
    {
        var escapedPattern = Regex.Escape(pattern.Pattern);

        var regex = escapedPattern.Replace("\\*", "(.*)", StringComparison.InvariantCultureIgnoreCase);
        RegexOptions options = RegexOptions.Singleline;
        if (ignoreCase)
        {
            options |= RegexOptions.IgnoreCase;
        }

        return Regex.IsMatch(value, $"^{regex}\\z", options); // Using \z to match end of string.
    }
}
