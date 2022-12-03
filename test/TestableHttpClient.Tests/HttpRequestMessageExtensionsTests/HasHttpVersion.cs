namespace TestableHttpClient.Tests;

public partial class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void HasHttpVersion_WithVersion_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion(HttpVersion.Version11));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasHttpVersion_WithString_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion("1.1"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasHttpVersion_WithVersion_NullVersion_ThrowsArgumentNullException()
    {
        using HttpRequestMessage sut = new()
        {
#if NETFRAMEWORK
            Version = new Version(0, 0)
#else
            Version = HttpVersion.Unknown
#endif
        };

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion((Version)null!));
        Assert.Equal("httpVersion", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void HasHttpVersion_WithString_NullVersion_ThrowsArgumentNullException(string httpVersion)
    {
        using HttpRequestMessage sut = new()
        {
#if NETFRAMEWORK
            Version = new Version(0, 0)
#else
            Version = HttpVersion.Unknown
#endif
        };

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpVersion(httpVersion));
        Assert.Equal("httpVersion", exception.ParamName);
    }

    [Fact]
    public void HasHttpVersion_WithVersion_CorrectVersion_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage { Version = HttpVersion.Version11 };

        Assert.True(sut.HasHttpVersion(HttpVersion.Version11));
    }

    [Fact]
    public void HasHttpVersion_WithString_CorrectVersion_ReturnsTrue()
    {
        using HttpRequestMessage sut = new() { Version = HttpVersion.Version11 };

        Assert.True(sut.HasHttpVersion("1.1"));
    }

    [Fact]
    public void HasHttpVersion_WithVersion_IncorrectVersion_ReturnsFalse()
    {
        using HttpRequestMessage sut = new() { Version = HttpVersion.Version11 };

        Assert.False(sut.HasHttpVersion(HttpVersion.Version10));
    }

    [Fact]
    public void HasHttpVersion_WithString_IncorrectVersion_ReturnsFalse()
    {
        using HttpRequestMessage sut = new() { Version = HttpVersion.Version11 };

        Assert.False(sut.HasHttpVersion("1.0"));
    }
}
