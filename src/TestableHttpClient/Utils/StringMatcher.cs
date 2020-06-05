using System.Text.RegularExpressions;

namespace TestableHttpClient.Utils
{
    internal static class StringMatcher
    {
        internal static bool Matches(string value, string pattern)
        {
            var regex = Regex.Escape(pattern).Replace("\\*", "(.*)");

            return Regex.IsMatch(value, $"^{regex}$");
        }
    }
}
