namespace TestableHttpClient.Utils;

internal static class HttpRequestMessageExtensions
{
    internal static Dictionary<string, string> GetHeaders(this HttpRequestMessage request)
    {
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> requestHeaders = request.Headers;
        IEnumerable<KeyValuePair<string, IEnumerable<string>>>? contentHeaders = request.Content?.Headers;
        return requestHeaders.Concat(contentHeaders ?? []).ToDictionary(x => x.Key, x => string.Join(" ", x.Value), StringComparer.OrdinalIgnoreCase);
    }

    internal static string? GetStringContent(this HttpRequestMessage request)
    {
        string? content = null;
        if(request.Content is not null)
        {
            content = request.Content.ReadAsStringAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        return content;
    }
}
