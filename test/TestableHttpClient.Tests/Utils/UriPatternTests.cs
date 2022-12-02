using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class UriPatternTests
{
    private readonly UriPatternMatchingOptions defaultOptions = new();

    [Theory]
    [InlineData("https://httpbin.com/get")]
    [InlineData("http://httpbin.com/post")]
    [InlineData("https://httpbin.com/get?test=test")]
    [InlineData("https://httpbin.com:5000/get?test=test")]
    public void Matches_AnyUriPattern_MatchesAllUrls(string uriString)
    {
        bool result = UriPattern.Any.Matches(new Uri(uriString), defaultOptions);
        Assert.True(result);
    }

    [Theory]
    [InlineData("https")]
    [InlineData("HTTPS")]
    public void Matches_SpecificScheme_MatchesAnyUrlWithThatProtocol(string expectedScheme)
    {
        UriPattern route = new() { Scheme = Value.Exact(expectedScheme) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.False(route.Matches(new Uri("http://httpbin.com/get"), defaultOptions));
    }

    [Theory]
    [InlineData("http*")]
    [InlineData("HTTP*")]
    public void Matches_PatternScheme_Matches_AnyUrlMatchingTheSchemePattern(string expectedSchemePattern)
    {
        UriPattern route = new() { Scheme = Value.Pattern(expectedSchemePattern) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(route.Matches(new Uri("http://httpbin.com/get"), defaultOptions));
        Assert.False(route.Matches(new Uri("ftp://httpbin.com/get"), defaultOptions));
    }

    [Theory]
    [InlineData("httpbin.com")]
    [InlineData("HTTPBIN.COM")]
    public void Matches_SpecificHost_MatchesAnyUrlWithThatHost(string expectedHost)
    {
        UriPattern route = new() { Host = Value.Exact(expectedHost) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.org/get"), defaultOptions));
    }

    [Theory]
    [InlineData("httpbin.*")]
    [InlineData("HTTPBIN.*")]
    public void Matches_PatternHost_MatchesAnyUrlWithThatHost(string expectedHostPattern)
    {
        UriPattern route = new() { Host = Value.Pattern(expectedHostPattern) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.org/get"), defaultOptions));
        Assert.False(route.Matches(new Uri("https://httpbin/get"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactPort_MatchesAnyUrlWithThatPort()
    {
        UriPattern uriPattern = new() { Port = Value.Exact("80") };

        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com"), defaultOptions));
        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com:80"), defaultOptions));
        Assert.False(uriPattern.Matches(new Uri("http://httpbin.com:5000"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternPort_MatchesAnyUrlWithThatPort()
    {
        UriPattern uriPattern = new() { Port = Value.Pattern("8*") };

        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com"), defaultOptions));
        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com:80"), defaultOptions));
        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com:8443"), defaultOptions));
        Assert.False(uriPattern.Matches(new Uri("http://httpbin.com:5000"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern route = new() { Path = Value.Exact("/get") };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get?query=test"), defaultOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.com/get/status"), defaultOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/GET"), defaultOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(route.Matches(new Uri("https://httpbin.com/post"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern route = new() { Path = Value.Pattern("/get*") };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get?query=test"), defaultOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get/status"), defaultOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/GET"), defaultOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(route.Matches(new Uri("https://httpbin.com/post"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactQuery_MatchesAnyUrlWithThatExactQuery()
    {
        UriPattern pattern = new() { Query = Value.Exact("key=value") };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=value"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternQuery_MatchesAnyUrlWithMatchinQuery()
    {
        UriPattern pattern = new() { Query = Value.Pattern("*=value") };
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=value"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=test&query=value"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?KEY=test&QUERY=value"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get?key=query"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get?key=test&query=VALUE"), new() { QueryCaseInsensitive = false }));
    }

    [Fact]
    public void Matches_ExactQueryWithSpecialCharacters_MatchesAccordingToUriFormat()
    {
        UriPattern pattern = new() { Query = Value.Exact("email=test@example.com") };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?email=test@example.com"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?email=test%40example.com"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?email=test@example.com"), new() { DefaultQueryFormat = UriFormat.UriEscaped }));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get?email=test%40example.com"), new() { DefaultQueryFormat = UriFormat.UriEscaped }));
    }
}
