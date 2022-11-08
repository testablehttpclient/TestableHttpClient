using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class RouteDefinitionTests
{
    [Theory]
    [InlineData("https://httpbin.com/get")]
    [InlineData("http://httpbin.com/post")]
    public void Matches_AnyRouteDefinition_MatchesAllUrls(string uriString)
    {
        bool result = RouteDefinition.Any.Matches(new Uri(uriString));
        Assert.True(result);
    }

    [Theory]
    [InlineData("https")]
    [InlineData("HTTPS")]
    public void Matches_SpecificScheme_MatchesAnyUrlWithThatProtocol(string expectedScheme)
    {
        RouteDefinition route = new() { Scheme = Value.Exact(expectedScheme) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get")));
        Assert.False(route.Matches(new Uri("http://httpbin.com/get")));
    }

    [Theory]
    [InlineData("http*")]
    [InlineData("HTTP*")]
    public void Matches_PatternScheme_Matches_AnyUrlMatchingTheSchemePattern(string expectedSchemePattern)
    {
        RouteDefinition route = new() { Scheme = Value.Pattern(expectedSchemePattern) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get")));
        Assert.True(route.Matches(new Uri("http://httpbin.com/get")));
        Assert.False(route.Matches(new Uri("ftp://httpbin.com/get")));
    }

    [Theory]
    [InlineData("httpbin.com")]
    [InlineData("HTTPBIN.COM")]
    public void Matches_SpecificHost_MatchesAnyUrlWithThatHost(string expectedHost)
    {
        RouteDefinition route = new() { Host = Value.Exact(expectedHost) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get")));
        Assert.False(route.Matches(new Uri("https://httpbin.org/get")));
    }

    [Theory]
    [InlineData("httpbin.*")]
    [InlineData("HTTPBIN.*")]
    public void Matches_PatternHost_MatchesAnyUrlWithThatHost(string expectedHostPattern)
    {
        RouteDefinition route = new() { Host = Value.Pattern(expectedHostPattern) };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get")));
        Assert.True(route.Matches(new Uri("https://httpbin.org/get")));
        Assert.False(route.Matches(new Uri("https://httpbin/get")));
    }

    [Fact]
    public void Matches_ExactPath_MatchesAnyUrlWithThatExactPath()
    {
        RouteDefinition route = new() { Path = Value.Exact("/get") };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get")));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get?query=test")));
        Assert.False(route.Matches(new Uri("https://httpbin.com/get/status")));
        Assert.False(route.Matches(new Uri("https://httpbin.com/GET")));
        Assert.False(route.Matches(new Uri("https://httpbin.com/post")));
    }

    [Fact]
    public void Matches_PatternPath_MatchesAnyUrlWithThatExactPath()
    {
        RouteDefinition route = new() { Path = Value.Pattern("/get*") };

        Assert.True(route.Matches(new Uri("https://httpbin.com/get")));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get?query=test")));
        Assert.True(route.Matches(new Uri("https://httpbin.com/get/status")));
        Assert.False(route.Matches(new Uri("https://httpbin.com/GET")));
        Assert.False(route.Matches(new Uri("https://httpbin.com/post")));
    }
}
