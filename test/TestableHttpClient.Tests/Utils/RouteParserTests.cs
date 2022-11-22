using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;
public class RouteParserTests
{
    [Theory]
    [InlineData("*")]
    [InlineData("*://*")]
    [InlineData("*/*")]
    [InlineData("*://*/*")]
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

    [Theory]
    [InlineData("*://httpbin.org/*")]
    [InlineData("*://httpbin.org")]
    [InlineData("httpbin.org")]
    public void Parse_ExactHost_ReturnsRouteDefintionWithExactHost(string input)
    {
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Exact("httpbin.org"), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("*://*.org/*", "*.org")]
    [InlineData("*://httpbin.*/*", "httpbin.*")]
    [InlineData("*://*.com", "*.com")]
    [InlineData("*.com", "*.com")]
    public void Parse_PatternHost_ReturnsRouteDefintionWithPatternHost(string input, string expectedPattern)
    {
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Pattern(expectedPattern), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("*/", "/")]
    [InlineData("*/get", "/get")]
    [InlineData("/get", "/get")]
    public void Parse_WildCardWithCompletePath_ReturnsRouteDefintionWithExactPath(string input, string expectedValue)
    {
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Exact(expectedValue), result.Path);
    }

    [Theory]
    [InlineData("*/get/*", "/get/*")]
    [InlineData("/post/*", "/post/*")]
    public void Parse_WildCardWithPatternPath_ReturnsRouteDefintionWithPatternPath(string input, string expectedPattern)
    {
        RouteDefinition result = RouteParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Pattern(expectedPattern), result.Path);
    }

    [Theory]
    [InlineData("")]
    public void Parse_InvalidInput_ThrowsRouteParserException(string input)
    {
        Assert.Throws<RouteParserException>(() => RouteParser.Parse(input));
    }
}

