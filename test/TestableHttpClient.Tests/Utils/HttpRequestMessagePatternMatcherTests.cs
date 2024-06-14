using FluentAssertions;

using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class HttpRequestMessagePatternMatcherTests
{
    [Fact]
    public void MultipleRequests_MatchingAnyPattern_ReturnsAllRequests()
    {
        HttpRequestMessage[] requestMessages = [
            new HttpRequestMessage(HttpMethod.Get, "https://localhost/get"),
            new HttpRequestMessage(HttpMethod.Post, "https://localhost/post"),
            new HttpRequestMessage(HttpMethod.Options, "https://localhost/options")
            ];

        HttpRequestMessagePattern pattern = new();

        HttpRequestMessagePatternMatcher matcher = new();

        HttpRequestMessagePatternMatchResult result = matcher.Match(requestMessages, pattern);

        result.MatchedRequests.Should().BeEquivalentTo(requestMessages);
        result.UnmatchedRequests.Should().BeEmpty();
    }

    [Fact]
    public void MultipleRequests_MatchingSpecificHttpMethod_ReturnsMatchinRequests()
    {
        HttpRequestMessage getRequest = new(HttpMethod.Get, "https://localhost/get");
        HttpRequestMessage postRequest = new(HttpMethod.Post, "https://localhost/post");
        HttpRequestMessage optionsRequest = new(HttpMethod.Options, "https://localhost/options");
        HttpRequestMessage[] requestMessages = [ getRequest, postRequest, optionsRequest ];

        HttpRequestMessagePattern pattern = new()
        {
            Method = Value.Exact(HttpMethod.Post)
        };

        HttpRequestMessagePatternMatcher matcher = new();

        HttpRequestMessagePatternMatchResult result = matcher.Match(requestMessages, pattern);

        result.MatchedRequests.Should().BeEquivalentTo([postRequest]);
        result.UnmatchedRequests.Should().BeEquivalentTo([getRequest, optionsRequest]);
    }
}
