using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;
public class UriPatternParserTests
{
    [Theory]
    [InlineData("*")]
    [InlineData("*://*")]
    [InlineData("*/*")]
    [InlineData("*://*/*")]
    public void Parse_WildCardPattern_ReturnsAnyUriPattern(string input)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Fact]
    public void Parse_ExactScheme_ReturnsUriPatternWithExactScheme()
    {
        string input = "https://*";
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Exact("https"), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Fact]
    public void Parse_ExactSchemeWithExactHost_ReturnsUriPatternWithExactSchemeAndHost()
    {
        string input = "https://httpbin.org";
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Exact("https"), result.Scheme);
        Assert.Equal(Value.Exact("httpbin.org"), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("http*://*", "http*")]
    [InlineData("*s://*", "*s")]
    public void Parse_PatternScheme_ReturnsUriPatternWithPatternScheme(string input, string expectedPattern)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Pattern(expectedPattern), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("*://httpbin.org/*")]
    [InlineData("*://httpbin.org")]
    [InlineData("httpbin.org")]
    public void Parse_ExactHost_ReturnsUriPatternWithExactHost(string input)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Exact("httpbin.org"), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("*://*.org/*", "*.org")]
    [InlineData("*://httpbin.*/*", "httpbin.*")]
    [InlineData("*://*.com", "*.com")]
    [InlineData("*.com", "*.com")]
    public void Parse_PatternHost_ReturnsUriPatternWithPatternHost(string input, string expectedPattern)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Pattern(expectedPattern), result.Host);
        Assert.Equal(Value.Any(), result.Path);
    }

    [Theory]
    [InlineData("*/", "/")]
    [InlineData("*/get", "/get")]
    [InlineData("/get", "/get")]
    public void Parse_WildCardWithCompletePath_ReturnsUriPatternWithExactPath(string input, string expectedValue)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Exact(expectedValue), result.Path);
    }

    [Theory]
    [InlineData("*/get/*", "/get/*")]
    [InlineData("/post/*", "/post/*")]
    public void Parse_WildCardWithPatternPath_ReturnsUriPatternWithPatternPath(string input, string expectedPattern)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Pattern(expectedPattern), result.Path);
    }

    [Theory]
    [InlineData("")]
    [InlineData("://")]
    public void Parse_InvalidInput_ThrowsUriPatternParserException(string input)
    {
        Assert.Throws<UriPatternParserException>(() => UriPatternParser.Parse(input));
    }
}

