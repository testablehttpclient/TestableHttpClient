using System.Text.RegularExpressions;

namespace TestableHttpClient.Utils;

internal static class StringMatcher
{
    internal static bool Matches(string value, string pattern, bool ignoreCase = false)
    {
        var escapedPattern = Regex.Escape(pattern);

        var regex = escapedPattern.Replace("\\*", "(.*)", StringComparison.InvariantCultureIgnoreCase);
        RegexOptions options = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
        return Regex.IsMatch(value, $"^{regex}$", options);
    }
}
