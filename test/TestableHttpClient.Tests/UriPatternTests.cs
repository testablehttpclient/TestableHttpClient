namespace TestableHttpClient.Tests;

public sealed class UriPatternTests
{
    private readonly UriPatternMatchingOptions defaultOptions = new();

    [Theory]
    [InlineData("https://httpbin.com/get")]
    [InlineData("http://httpbin.com/post")]
    [InlineData("https://httpbin.com/get?test=test")]
    [InlineData("https://httpbin.com:5000/get?test=test")]
    public void Matches_AnyUriPattern_MatchesAllUrls(string uriString)
    {
        bool result = new UriPattern().Matches(new Uri(uriString), defaultOptions);
        Assert.True(result);
    }

    [Theory]
    [InlineData("https")]
    [InlineData("HTTPS")]
    public void Matches_SpecificScheme_MatchesAnyUrlWithThatProtocol(string expectedScheme)
    {
        UriPattern pattern = new() { Scheme = new ExactValue(expectedScheme) };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("http://httpbin.com/get"), defaultOptions));
    }

    [Theory]
    [InlineData("http*")]
    [InlineData("HTTP*")]
    public void Matches_PatternScheme_Matches_AnyUrlMatchingTheSchemePattern(string expectedSchemePattern)
    {
        UriPattern pattern = new() { Scheme = new PatternValue(expectedSchemePattern) };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("http://httpbin.com/get"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("ftp://httpbin.com/get"), defaultOptions));
    }

    [Theory]
    [InlineData("httpbin.com")]
    [InlineData("HTTPBIN.COM")]
    public void Matches_SpecificHost_MatchesAnyUrlWithThatHost(string expectedHost)
    {
        UriPattern pattern = new() { Host = new ExactValue(expectedHost) };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.org/get"), defaultOptions));
    }

    [Theory]
    [InlineData("httpbin.*")]
    [InlineData("HTTPBIN.*")]
    public void Matches_PatternHost_MatchesAnyUrlWithThatHost(string expectedHostPattern)
    {
        UriPattern pattern = new() { Host = new PatternValue(expectedHostPattern) };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.org/get"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin/get"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactPort_MatchesAnyUrlWithThatPort()
    {
        UriPattern uriPattern = new() { Port = new ExactValue("80") };

        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com"), defaultOptions));
        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com:80"), defaultOptions));
        Assert.False(uriPattern.Matches(new Uri("http://httpbin.com:5000"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternPort_MatchesAnyUrlWithThatPort()
    {
        UriPattern uriPattern = new() { Port = new PatternValue("8*") };

        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com"), defaultOptions));
        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com:80"), defaultOptions));
        Assert.True(uriPattern.Matches(new Uri("http://httpbin.com:8443"), defaultOptions));
        Assert.False(uriPattern.Matches(new Uri("http://httpbin.com:5000"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern pattern = new() { Path = new ExactValue("/get") };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?query=test"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get/status"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/GET"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/post"), defaultOptions));

        Assert.False(new UriPattern { Path = new ExactValue("/test/../example") }.Matches(new Uri("http://httpbin.com/example"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern pattern = new() { Path = new PatternValue("/get*") };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?query=test"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get/status"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/GET"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/post"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactQuery_MatchesAnyUrlWithThatExactQuery()
    {
        UriPattern pattern = new() { Query = new ExactValue("key=value") };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=value"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternQuery_MatchesAnyUrlWithMatchinQuery()
    {
        UriPattern pattern = new() { Query = new PatternValue("*=value") };
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=value"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=test&query=value"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?KEY=test&QUERY=value"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get?key=query"), defaultOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get?key=test&query=VALUE"), new() { QueryCaseInsensitive = false }));
    }

    [Fact]
    public void Matches_ExactQueryWithSpecialCharacters_MatchesAccordingToUriFormat()
    {
        UriPattern pattern = new() { Query = new ExactValue("email=test@example.com") };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?email=test@example.com"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?email=test%40example.com"), defaultOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?email=test@example.com"), new() { DefaultQueryFormat = UriFormat.UriEscaped }));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get?email=test%40example.com"), new() { DefaultQueryFormat = UriFormat.UriEscaped }));
    }
}
