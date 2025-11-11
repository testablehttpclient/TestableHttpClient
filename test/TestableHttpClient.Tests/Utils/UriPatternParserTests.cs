using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class UriPatternParserTests
{
    [Theory]
    [InlineData("*")]
    [InlineData("*://*")]
    [InlineData("*://*:*")]
    [InlineData("*/*")]
    [InlineData("*://*/*")]
    [InlineData("*://*/*?*")]
    [InlineData("://")]
    [InlineData("?")]
    public void Parse_WildCardPattern_ReturnsAnyUriPattern(string input)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Fact]
    public void Parse_ExactScheme_ReturnsUriPatternWithExactScheme()
    {
        string input = "https://*";
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Exact("https"), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Fact]
    public void Parse_ExactSchemeWithExactHost_ReturnsUriPatternWithExactSchemeAndHost()
    {
        string input = "https://httpbin.org";
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Exact("https"), result.Scheme);
        Assert.Equal(Value.Exact("httpbin.org"), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("http*://*", "http*")]
    [InlineData("*s://*", "*s")]
    public void Parse_PatternScheme_ReturnsUriPatternWithPatternScheme(string input, string expectedPattern)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Pattern(expectedPattern), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("*://httpbin.org/*?*", "httpbin.org")]
    [InlineData("*://httpbin.org/*", "httpbin.org")]
    [InlineData("*://httpbin.org", "httpbin.org")]
    [InlineData("//httpbin.org", "httpbin.org")]
    [InlineData("//user:pass@httpbin.org", "httpbin.org")]
    [InlineData("httpbin.org", "httpbin.org")]
    [InlineData("127.0.0.1", "127.0.0.1")]
    [InlineData("[::1]", "[::1]")]
    public void Parse_ExactHost_ReturnsUriPatternWithExactHost(string input, string expectedValue)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Exact(expectedValue), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("*://*.org/*", "*.org")]
    [InlineData("//httpbin.*/*", "httpbin.*")]
    [InlineData("//user:pass@httpbin.*/*", "httpbin.*")]
    [InlineData("*://httpbin.*/*", "httpbin.*")]
    [InlineData("*://*.com", "*.com")]
    [InlineData("*.com", "*.com")]
    public void Parse_PatternHost_ReturnsUriPatternWithPatternHost(string input, string expectedPattern)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Pattern(expectedPattern), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("*://*:8443/*?*")]
    [InlineData("*://*:8443/*")]
    [InlineData("*://*:8443")]
    [InlineData("//*:8443")]
    [InlineData("*:8443")]
    public void Parse_ExactPort_ReturnsUriPatternWithExactHost(string input)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Exact("8443"), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("*://*:8*/*?*")]
    [InlineData("*://*:8*/*")]
    [InlineData("*://*:8*")]
    [InlineData("//*:8*")]
    [InlineData("*:8*")]
    public void Parse_PatternPort_ReturnsUriPatternWithPatternHost(string input)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Pattern("8*"), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("/?", "/")]
    [InlineData("*/", "/")]
    [InlineData("*/get", "/get")]
    [InlineData("/get", "/get")]
    [InlineData("/get?*", "/get")]
    public void Parse_WildCardWithCompletePath_ReturnsUriPatternWithExactPath(string input, string expectedValue)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Exact(expectedValue), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("*/get/*", "/get/*")]
    [InlineData("/post/*", "/post/*")]
    public void Parse_WildCardWithPatternPath_ReturnsUriPatternWithPatternPath(string input, string expectedPattern)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Pattern(expectedPattern), result.Path);
        Assert.Equal(Value.Any(), result.Query);
    }

    [Theory]
    [InlineData("/*?query=true", "query=true")]
    public void Parse_ExactQuery_ReturnsUriPatternWithExactQuery(string input, string expectedValue)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Exact(expectedValue), result.Query);
    }

    [Theory]
    [InlineData("/*?query=*", "query=*")]
    [InlineData("/*?*=true", "*=true")]
    public void Parse_PatternQuery_ReturnsUriPatternWithPatternQuery(string input, string expectedPattern)
    {
        UriPattern result = UriPatternParser.Parse(input);
        Assert.Equal(Value.Any(), result.Scheme);
        Assert.Equal(Value.Any(), result.Host);
        Assert.Equal(Value.Any(), result.Port);
        Assert.Equal(Value.Any(), result.Path);
        Assert.Equal(Value.Pattern(expectedPattern), result.Query);
    }

    [Theory]
    [InlineData("")]
    [InlineData("*:")]
    public void Parse_InvalidInput_ThrowsUriPatternParserException(string input)
    {
        Assert.Throws<UriPatternParserException>(() => UriPatternParser.Parse(input));
    }

    [Theory]
    [InlineData("httpbin.com/?query=*", "http://httpbin.com:8080/?query=test", "http://httpbin.com/test?query=test")]
    [InlineData("httpbin.com?query=*", "http://httpbin.com?query=test", "http://httpbin.com?test=query")]
    [InlineData("https://httpbin.com:8443", "https://httpbin.com:8443/", "https://httpbin.com/")]
    public void RoundTripTests(string pattern, string matchingUri, string notMatchinUri)
    {
        UriPattern uriPattern = UriPatternParser.Parse(pattern);
        Assert.True(uriPattern.Matches(new Uri(matchingUri), new()));
        Assert.False(uriPattern.Matches(new Uri(notMatchinUri), new()));
    }
}

