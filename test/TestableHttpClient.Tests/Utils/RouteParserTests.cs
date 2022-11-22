using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;
public class RouteParserTests
{
    [Theory]
    [InlineData("*")]
    [InlineData("*://*")]
    [InlineData("*/*")]
    public void Parse_WildCardPattern_ReturnsAnyRouteDefinition(string input)
    {
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Fact]
    public void Parse_ExactScheme_ReturnsRouteDefinitionWithExactScheme()
    {
        string input = "https://*";
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Exact("https"), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("http*://*", "http*")]
    [InlineData("*s://*", "*s")]
    public void Parse_PatternScheme_ReturnsRouteDefintionWithPatternScheme(string input, string expectedPattern)
    {
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Pattern(expectedPattern), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Fact]
    public void Parse_ExactHost_ReturnsRouteDefintionWithExactHost()
    {
        string input = "*://httpbin.org/*";
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Exact("httpbin.org"), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("*://*.org/*", "*.org")]
    [InlineData("*://httpbin.*/*", "httpbin.*")]
    public void Parse_PatternHost_ReturnsRouteDefintionWithPatternHost(string input, string expectedPattern)
    {
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Pattern(expectedPattern), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Fact]
    public void Parse_WildCardWithCompletePath_ReturnsRouteDefintionWithExactPath()
    {
        string input = "*/get";
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Exact("/get"), result.Path);
    }

    [Fact]
    public void Parse_WildCardWithPatternPath_ReturnsRouteDefintionWithPatternPath()
    {
        string input = "*/get/*";
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Pattern("/get/*"), result.Path);
    }
}

