namespace TestableHttpClient.Utils;

internal sealed class HttpRequestMessagePattern
{
    public Value<HttpMethod> Method { get; init; } = Value.Any<HttpMethod>();
    public UriPattern RequestUri { get; init; } = UriPattern.Any;
    public Value<Version> Version { get; init; } = Value.Any<Version>();
    public Dictionary<Value<string>, Value<string>> Headers { get; init; } = new() { [Value.Any<string>()] = Value.Any<string>() };
    public Value<string> Content { get; init; } = Value.Any<string>();

    public HttpRequestMessagePatternMatchingResult Matches(HttpRequestMessage httpRequestMessage, HttpRequestMessagePatternMatchingOptions options) =>
        new()
        {
            Method = Method.Matches(httpRequestMessage.Method, false),
            RequestUri = RequestUri.Matches(httpRequestMessage.RequestUri, options.RequestUriMatchingOptions),
            Version = Version.Matches(httpRequestMessage.Version, false),
            Headers = MatchesHeaders(httpRequestMessage),
            Content = MatchesContent(httpRequestMessage.Content)
        };

    private bool MatchesHeaders(HttpRequestMessage httpRequestMessage)
    {
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> requestHeaders = httpRequestMessage.Headers;
        if (httpRequestMessage.Content is not null)
        {
            requestHeaders = requestHeaders.Concat(httpRequestMessage.Content.Headers);
        }

        return MatchesHeaders(requestHeaders.ToDictionary(x => x.Key, x => string.Join(", ", x.Value), StringComparer.OrdinalIgnoreCase));
    }

    private bool MatchesHeaders(Dictionary<string, string> requestHeaders)
    {
        if (Headers.Count == 0 && requestHeaders.Count > 0)
        {
            return false;
        }

        foreach (var header in Headers)
        {
            bool matched = false;

            foreach (var requestHeader in requestHeaders)
            {
                matched |= header.Key.Matches(requestHeader.Key, true) && header.Value.Matches(requestHeader.Value, true);
            }

            if (!matched)
            {
                return false;
            }
        }

        return true;
    }

    private bool MatchesContent(HttpContent? requestContent)
    {
        if (requestContent == null)
        {
            return Content == Value.Any<string>();
        }

        var contentValue = requestContent.ReadAsStringAsync().Result;

        return Content.Matches(contentValue, false);
    }
}

internal sealed class HttpRequestMessagePatternMatchingOptions
{
    public UriPatternMatchingOptions RequestUriMatchingOptions { get; init; } = new();
}

internal sealed class HttpRequestMessagePatternMatchingResult
{
    public bool Method { get; init; }
    public bool RequestUri { get; init; }
    public bool Version { get; init; }
    public bool Headers { get; init; }
    public bool Content { get; init; }
}

