using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class UriPatternTests
{
    private readonly RoutingOptions defaultRoutingOptions = new();

    [Theory]
    [InlineData("https://httpbin.com/get")]
    [InlineData("http://httpbin.com/post")]
    [InlineData("https://httpbin.com/get?test=test")]
    public void Matches_AnyUriPattern_MatchesAllUrls(string uriString)
    {
        bool result = UriPattern.Any.Matches(new Uri(uriString), defaultRoutingOptions);
        Assert.True(result);
    }

    [Theory]
    [InlineData("https")]
    [InlineData("HTTPS")]
    public void Matches_SpecificScheme_MatchesAnyUrlWithThatProtocol(string expectedScheme)
    {
        UriPattern route = new() { Scheme = Value.Exact(expectedScheme) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultRoutingOptions));
        Assert.False(route.Matches(new Uri("http://httpbin.com/get"), defaultRoutingOptions));
    }

    [Theory]
    [InlineData("http*")]
    [InlineData("HTTP*")]
    public void Matches_PatternScheme_Matches_AnyUrlMatchingTheSchemePattern(string expectedSchemePattern)
    {
        UriPattern route = new() { Scheme = Value.Pattern(expectedSchemePattern) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultRoutingOptions));
        Assert.True(route.Matches(new Uri("http://httpbin.com/get"), defaultRoutingOptions));
        Assert.False(route.Matches(new Uri("ftp://httpbin.com/get"), defaultRoutingOptions));
    }

    [Theory]
    [InlineData("httpbin.com")]
    [InlineData("HTTPBIN.COM")]
    public void Matches_SpecificHost_MatchesAnyUrlWithThatHost(string expectedHost)
    {
        UriPattern route = new() { Host = Value.Exact(expectedHost) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultRoutingOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.org/get"), defaultRoutingOptions));
    }

    [Theory]
    [InlineData("httpbin.*")]
    [InlineData("HTTPBIN.*")]
    public void Matches_PatternHost_MatchesAnyUrlWithThatHost(string expectedHostPattern)
    {
        UriPattern route = new() { Host = Value.Pattern(expectedHostPattern) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultRoutingOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.org/get"), defaultRoutingOptions));
        Assert.False(route.Matches(new Uri("https://httpbin/get"), defaultRoutingOptions));
    }

    [Fact]
    public void Matches_ExactPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern route = new() { Path = Value.Exact("/get") };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultRoutingOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get?query=test"), defaultRoutingOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.com/get/status"), defaultRoutingOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/GET"), defaultRoutingOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(route.Matches(new Uri("https://httpbin.com/post"), defaultRoutingOptions));
    }

    [Fact]
    public void Matches_PatternPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern route = new() { Path = Value.Pattern("/get*") };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get"), defaultRoutingOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get?query=test"), defaultRoutingOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get/status"), defaultRoutingOptions));
        Assert.True(route.Matches(new Uri("https://httpbin.com/GET"), defaultRoutingOptions));
        Assert.False(route.Matches(new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(route.Matches(new Uri("https://httpbin.com/post"), defaultRoutingOptions));
    }

    [Fact]
    public void Matches_ExactQuery_MatchesAnyUrlWithThatExactQuery()
    {
        UriPattern pattern = new() { Query = Value.Exact("?key=value") };

        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=value"), defaultRoutingOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get"), defaultRoutingOptions));
    }

    [Fact]
    public void Matches_PatternQuery_MatchesAnyUrlWithMatchinQuery()
    {
        UriPattern pattern = new() { Query = Value.Pattern("?*=value") };
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=value"), defaultRoutingOptions));
        Assert.True(pattern.Matches(new Uri("https://httpbin.com/get?key=test&query=value"), defaultRoutingOptions));
        Assert.False(pattern.Matches(new Uri("https://httpbin.com/get?key=query"), defaultRoutingOptions));
    }
}
