namespace TestableHttpClient;

internal static class HttpHeadersExtensions
{
    internal static bool HasHeader(this HttpHeaders headers, string headerName, Value headerValue)
    {
        if (headers.TryGetValues(headerName, out var values))
        {
            var value = string.Join(" ", values);
            return headerValue.Matches(value, false);
        }
        return false;
    }
}
