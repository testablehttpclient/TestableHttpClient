namespace TestableHttpClient.Utils;

internal class HttpRequestMessagePatternMatcher
{
    private readonly HttpRequestMessagePatternMatchingOptions options = new();
    public HttpRequestMessagePatternMatchResult Match(IEnumerable<HttpRequestMessage> requests, HttpRequestMessagePattern pattern)
    {
        HttpRequestMessagePatternMatchResult result = new();

        foreach (HttpRequestMessage request in requests)
        {
            HttpRequestMessagePatternMatchingResult match = pattern.Matches(request, options);
            if (match.All)
            {
                result.MatchedRequests.Add(request);
            }
            else
            {
                result.UnmatchedRequests.Add(request);
            }
        }
        return result;
    }
}

internal class HttpRequestMessagePatternMatchResult
{
    public List<HttpRequestMessage> MatchedRequests { get; } = new List<HttpRequestMessage>();
    public List<HttpRequestMessage> UnmatchedRequests { get; } = new List<HttpRequestMessage>();
}
