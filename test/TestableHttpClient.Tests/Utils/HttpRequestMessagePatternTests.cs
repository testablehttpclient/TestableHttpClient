using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class HttpRequestMessagePatternTests
{
    private readonly HttpRequestMessagePatternMatchingOptions defaultOptions = new();

    [Theory]
    [InlineData("GET")]
    [InlineData("POST")]
    [InlineData("PATCH")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("HEAD")]
    [InlineData("OPTIONS")]
    [InlineData("TRACE")]
    [InlineData("CONNECT")]
    public void Matches_HttpRequestMessageWithAnyHttpMethod_MatchesAllHttpMethods(string httpMethod)
    {
        HttpRequestMessagePattern sut = new();

        using HttpRequestMessage input = new(new HttpMethod(httpMethod), "https://localhost");

        Assert.True(sut.Matches(input, defaultOptions).Method);
    }

    [Theory]
    [InlineData("GET", true)]
    [InlineData("POST", true)]
    [InlineData("PATCH", false)]
    [InlineData("PUT", false)]
    [InlineData("DELETE", true)]
    [InlineData("HEAD", false)]
    [InlineData("OPTIONS", false)]
    [InlineData("TRACE", false)]
    [InlineData("CONNECT", false)]
    public void Matches_HttpRequestMessageWithSpecificHttpMethods_OnlyMatchesSpecifiedHttpMethods(string httpMethod, bool match)
    {
        HttpRequestMessagePattern sut = new()
        {
            Method = Value.OneOf(HttpMethod.Get, HttpMethod.Post, HttpMethod.Delete)
        };

        using HttpRequestMessage input = new(new HttpMethod(httpMethod), "https://localhost");

        Assert.Equal(match, sut.Matches(input, defaultOptions).Method);
    }

    [Fact]
    public void Matches_HttpRequestMessageWithSpecificUrl_MatchesUrl()
    {
        HttpRequestMessagePattern sut = new()
        {
            RequestUri = UriPatternParser.Parse("https://localhost/test/*")
        };

        using HttpRequestMessage matchingInput = new(HttpMethod.Get, "https://localhost/test/123");
        using HttpRequestMessage notMatchingInput = new(HttpMethod.Get, "https://localhost/something/123");

        Assert.True(sut.Matches(matchingInput, defaultOptions).RequestUri);
        Assert.False(sut.Matches(notMatchingInput, defaultOptions).RequestUri);
    }

    [Fact]
    public void Matches_HttpRequestMessageWithSpecificVersion_MatchesExactVersion()
    {
        HttpRequestMessagePattern sut = new()
        {
            Version = Value.Exact(HttpVersion.Version11)
        };

        using HttpRequestMessage matchingVersion = new() { Version = HttpVersion.Version11 };
        using HttpRequestMessage notMatchingVersion = new() { Version = HttpVersion.Version10 };

        Assert.True(sut.Matches(matchingVersion, defaultOptions).Version);
        Assert.False(sut.Matches(notMatchingVersion, defaultOptions).Version);
    }

    [Fact]
    public void Matches_HttpRequestMessageWithoutBody_MatchesAnyBody()
    {
        HttpRequestMessagePattern sut = new()
        {
            Content = Value.Any<string>()
        };

        using HttpRequestMessage matchingRequest = new();

        Assert.True(sut.Matches(matchingRequest, defaultOptions).Content);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void Matches_HttpRequestMessageWithBody_MatchesExactBody(string content)
    {
        HttpRequestMessagePattern sut = new()
        {
            Content = Value.Exact(content)
        };

        using HttpRequestMessage matchingRequest = new()
        {
            Content = new StringContent(content)
        };

        Assert.True(sut.Matches(matchingRequest, defaultOptions).Content);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void Matches_HttpRequestMessageWithNotMatchingBody_DoesNotMatchExactBody(string content)
    {
        HttpRequestMessagePattern sut = new()
        {
            Content = Value.Exact(content)
        };

        using HttpRequestMessage matchingRequest = new()
        {
            Content = new StringContent("Example content")
        };

        Assert.False(sut.Matches(matchingRequest, defaultOptions).Content);
    }
}
