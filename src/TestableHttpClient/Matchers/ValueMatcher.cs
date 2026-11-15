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
}
