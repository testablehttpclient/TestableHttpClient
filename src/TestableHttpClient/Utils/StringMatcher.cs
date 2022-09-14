using System.Text.RegularExpressions;

namespace TestableHttpClient.Utils;

internal static class StringMatcher
{
    internal static bool Matches(string value, string pattern)
    {
        var escapedPattern = Regex.Escape(pattern);
#if NETFRAMEWORK
        var regex = escapedPattern.Replace("\\*", "(.*)");
#else
        var regex = escapedPattern.Replace("\\*", "(.*)", StringComparison.InvariantCultureIgnoreCase);
#endif

        return Regex.IsMatch(value, $"^{regex}$");
    }
}
