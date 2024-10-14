using FluentAssertions;

using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class HttpRequestMessagePatternMatcherTests
{
    [Fact]
    public void MultipleRequests_MatchingAnyPattern_ReturnsAllRequests()
    {
        using HttpRequestMessage getRequest = new(HttpMethod.Get, "https://localhost/get");
        using HttpRequestMessage postRequest = new(HttpMethod.Post, "https://localhost/post");
        using HttpRequestMessage optionsRequest = new(HttpMethod.Options, "https://localhost/options");
        HttpRequestMessage[] requestMessages = [getRequest, postRequest, optionsRequest];

        HttpRequestMessagePattern pattern = new();

        HttpRequestMessagePatternMatcher matcher = new();

        HttpRequestMessagePatternMatchResult result = matcher.Match(requestMessages, pattern);

        result.MatchedRequests.Should().BeEquivalentTo(requestMessages);
        result.UnmatchedRequests.Should().BeEmpty();
    }

    [Fact]
    public void MultipleRequests_MatchingSpecificHttpMethod_ReturnsMatchinRequests()
    {
        using HttpRequestMessage getRequest = new(HttpMethod.Get, "https://localhost/get");
        using HttpRequestMessage postRequest = new(HttpMethod.Post, "https://localhost/post");
        using HttpRequestMessage optionsRequest = new(HttpMethod.Options, "https://localhost/options");
        HttpRequestMessage[] requestMessages = [ getRequest, postRequest, optionsRequest ];

        HttpRequestMessagePattern pattern = new()
        {
            Method = Value.Exact(HttpMethod.Post)
        };

        HttpRequestMessagePatternMatcher matcher = new();

        HttpRequestMessagePatternMatchResult result = matcher.Match(requestMessages, pattern);

        result.MatchedRequests.Should().BeEquivalentTo([postRequest]);
        result.UnmatchedRequests.Should().BeEquivalentTo([
            new HttpRequestMessagePatternMatchingResult{
                RequestMessage = getRequest,
                Method = false,
                RequestUri = true,
                Version = true,
                Headers = true,
                Content = true
            }, new HttpRequestMessagePatternMatchingResult{
                RequestMessage = optionsRequest,
                Method = false,
                RequestUri = true,
                Version = true,
                Headers = true,
                Content = true
            }]);
    }
}
