﻿namespace TestableHttpClient.Tests;

[Obsolete("Use WithRequestUri instead, since it now properly supports QueryStrings as well")]
public partial class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void HasQueryString_NullHttpRequestMessage_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasQueryString("lang=en"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasQueryString_NullPattern_ThrowsArgumentNullException()
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com?lang=en") };
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasQueryString(null!));
        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void HasQueryString_QueryInUrlButEmptyPattern_ReturnsFalse()
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com?lang=en") };

        Assert.False(sut.HasQueryString(string.Empty));
    }

    [Fact]
    public void HasQueryString_NullRequestUri_ReturnsFalse()
    {
        using HttpRequestMessage sut = new() { RequestUri = null };

        Assert.False(sut.HasQueryString("*"));
    }

    [Fact]
    public void HasQueryString_MatchingEmptyPattern_ReturnsTrue()
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com") };

        Assert.True(sut.HasQueryString(string.Empty));
    }

    [Fact]
    public void HasQueryString_NoQueryInRequestAndWildcardPattern_ReturnsTrue()
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com") };

        Assert.True(sut.HasQueryString("*"));
    }

    [Fact]
    public void HasQueryString_NoQueryInUrlButExactPattern_ReturnsFalse()
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com") };

        Assert.False(sut.HasQueryString("lang=en"));
    }

    [Fact]
    public void HasQueryString_QueryInUrlAndExactPattern_ReturnsTrue()
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com?lang=en") };

        Assert.True(sut.HasQueryString("lang=en"));
    }

    [Fact]
    public void HasQueryString_UrlEncodedQueryInUrlAndExactPattern_ReturnsTrue()
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com?email=test%40example.com") };

        Assert.True(sut.HasQueryString("email=test@example.com"));
    }

    [Theory]
    [InlineData("lang=*")]
    [InlineData("lang=*&email=*")]
    [InlineData("*email=*")]
    public void HasQueryString_MatchingPatternContainingWildcard_ReturnsTrue(string pattern)
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com?lang=en&email=test%40example.com") };

        Assert.True(sut.HasQueryString(pattern));
    }

    [Theory]
    [InlineData("lang=nl*")]
    [InlineData("lang=*&uri=*")]
    [InlineData("*uri=*")]
    public void HasQueryString_NotMatchingPatternContainingWildcard_ReturnsFalse(string pattern)
    {
        using HttpRequestMessage sut = new() { RequestUri = new Uri("https://example.com?lang=en&email=test%40example.com") };

        Assert.False(sut.HasQueryString(pattern));
    }
}
