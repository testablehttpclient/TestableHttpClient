namespace TestableHttpClient.Utils;

internal sealed class HttpRequestMessagePattern
{
    public Value<HttpMethod> Method { get; init; } = Value.Any<HttpMethod>();
    public UriPattern RequestUri { get; init; } = UriPattern.Any;
    public Value<Version> Version { get; init; } = Value.Any<Version>();

    // public ??? Headers { get; init; }
    public Value<string> Content { get; init; } = Value.Any<string>();

    public HttpRequestMessagePatternMatchingResult Matches(HttpRequestMessage httpRequestMessage, HttpRequestMessagePatternMatchingOptions options) =>
        new()
        {
            Method = Method.Matches(httpRequestMessage.Method, false),
            RequestUri = RequestUri.Matches(httpRequestMessage.RequestUri, options.RequestUriMatchingOptions),
            Version = Version.Matches(httpRequestMessage.Version, false),
            Content = MatchesContent(httpRequestMessage.Content)
        };

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
    public bool Content { get; init; }
}

