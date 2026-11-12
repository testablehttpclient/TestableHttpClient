using System.Diagnostics;

namespace TestableHttpClient;

internal static class HttpHeadersExtensions
{
    internal static bool HasHeader(this HttpHeaders headers, string headerName, HeaderValue headerValue)
    {
        if (!headers.TryGetValues(headerName, out var values))
        {
            return false;
        }

        var value = string.Join(" ", values);
        return headerValue switch
        {
            Any => true,
            Pattern pattern => StringMatcher.Matches(value, pattern.pattern, false),
            _ => throw new UnreachableException()
        };
    }
}
