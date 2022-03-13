namespace TestableHttpClient.Tests;

public partial class HttpRequestMessageExtensionsTests
{
#nullable disable
    [Fact]
    public void HasQueryString_NullHttpRequestMessage_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasQueryString("lang=en"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasQueryString_NullPattern_ThrowsArgumentNullException()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com?lang=en") };
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasQueryString(null));
        Assert.Equal("pattern", exception.ParamName);
    }
#nullable restore

    [Fact]
    public void HasQueryString_QueryInUrlButEmptyPattern_ReturnsFalse()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com?lang=en") };

        Assert.False(sut.HasQueryString(string.Empty));
    }

    [Fact]
    public void HasQueryString_MatchingEmptyPattern_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com") };

        Assert.True(sut.HasQueryString(string.Empty));
    }

    [Fact]
    public void HasQueryString_NoQueryInRequestAndWildcardPattern_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com") };

        Assert.True(sut.HasQueryString("*"));
    }

    [Fact]
    public void HasQueryString_NoQueryInUrlButExactPattern_ReturnsFalse()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com") };

        Assert.False(sut.HasQueryString("lang=en"));
    }

    [Fact]
    public void HasQueryString_QueryInUrlAndExactPattern_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com?lang=en") };

        Assert.True(sut.HasQueryString("lang=en"));
    }

    [Fact]
    public void HasQueryString_UrlEncodedQueryInUrlAndExactPattern_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com?email=test%40example.com") };

        Assert.True(sut.HasQueryString("email=test@example.com"));
    }

    [Theory]
    [InlineData("lang=*")]
    [InlineData("lang=*&email=*")]
    [InlineData("*email=*")]
    public void HasQueryString_MatchingPatternContainingWildcard_ReturnsTrue(string pattern)
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com?lang=en&email=test%40example.com") };

        Assert.True(sut.HasQueryString(pattern));
    }

    [Theory]
    [InlineData("lang=nl*")]
    [InlineData("lang=*&uri=*")]
    [InlineData("*uri=*")]
    public void HasQueryString_NotMatchingPatternContainingWildcard_ReturnsFalse(string pattern)
    {
        using var sut = new HttpRequestMessage { RequestUri = new Uri("https://example.com?lang=en&email=test%40example.com") };

        Assert.False(sut.HasQueryString(pattern));
    }
}
