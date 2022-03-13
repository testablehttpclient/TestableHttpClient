namespace TestableHttpClient.Tests;

public partial class HttpRequestMessageExtensionsTests
{
#nullable disable
    [Fact]
    public void HasMatchingUri_NullHttpRequestMessage_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasMatchingUri("*"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasMatchingUri_NullPattern_ThrowsArgumentNullException()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com") };
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasMatchingUri(null));
        Assert.Equal("pattern", exception.ParamName);
    }
#nullable restore

    [Fact]
    public void HasMatchingUri_EmptyPattern_ReturnsFalse()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com") };

        Assert.False(sut.HasMatchingUri(""));
    }

    [Theory]
    [InlineData("*")]
    [InlineData("https://example.com*")]
    [InlineData("https://example.com/")]
    [InlineData("https://example.com/*")]
    [InlineData("https://*/")]
    [InlineData("https://*/*")]
    public void HasMatchingUri_WithPatternFor_ReturnsTrue(string pattern)
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com") };

        Assert.True(sut.HasMatchingUri(pattern));
    }

    [Fact]
    public void HasMatchingUri_PatternWithMultipleWildCards_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com/api/test/value") };

        Assert.True(sut.HasMatchingUri("https://*/api/*"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("http://example.com")]
    [InlineData("https://example.com")] // Missing slash at the end.
    [InlineData("http://*")]
    [InlineData("https://*/api/test")]
    public void HasMatchingUri_WithPatternThatDoesnotMatch_ReturnsFalse(string pattern)
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com") };

        Assert.False(sut.HasMatchingUri(pattern));
    }

    [Fact]
    public void HasMatchingUri_WithValidPatternIncludingQueryParameters_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com/list?id=test") };

        Assert.True(sut.HasMatchingUri("*/list?id=test"));
    }
}
