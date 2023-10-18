namespace TestableHttpClient.Tests;

public partial class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void HasRequestHeader_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader("host"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_NullHeaderName_ThrowsArgumentNullException()
    {
        using HttpRequestMessage sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader(null!));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_EmptyHeaderName_ThrowsArgumentException()
    {
        using HttpRequestMessage sut = new();

        var exception = Assert.Throws<ArgumentException>(() => sut.HasRequestHeader(string.Empty));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_NullRequestNonNullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader("host", "value"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_NullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException()
    {
        using HttpRequestMessage sut = new();
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader(null!, "value"));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_EmptyHeaderNameAndNonNullHeaderValue_ThrowsArgumentException()
    {
        using HttpRequestMessage sut = new();
        var exception = Assert.Throws<ArgumentException>(() => sut.HasRequestHeader(string.Empty, "value"));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_NonNullHeaderNameAndNullHeaderValue_ThrowsArgumentNullException()
    {
        using HttpRequestMessage sut = new();
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasRequestHeader("Host", null!));
        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_NonNullHeaderNameAndEmptyHeaderValue_ThrowsArgumentException()
    {
        using HttpRequestMessage sut = new();
        var exception = Assert.Throws<ArgumentException>(() => sut.HasRequestHeader("Host", string.Empty));
        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void HasRequestHeader_ExistingHeaderName_ReturnsTrue()
    {
        using HttpRequestMessage sut = new();
        sut.Headers.Host = "example.com";

        Assert.True(sut.HasRequestHeader("Host"));
    }

    [Theory]
    [InlineData("Host")]
    [InlineData("Content-Type")]
    public void HasRequestHeader_NotExistingHeaderName_ReturnsFalse(string headerName)
    {
        using HttpRequestMessage sut = new();

        Assert.False(sut.HasRequestHeader(headerName));
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("example*")]
    [InlineData("*.com")]
    [InlineData("*")]
    public void HasRequestHeader_ExistingHeaderNameMatchingValue_ReturnsTrue(string value)
    {
        using HttpRequestMessage sut = new();
        sut.Headers.Host = "example.com";

        Assert.True(sut.HasRequestHeader("Host", value));
    }

    [Fact]
    public void HasRequestHeader_NotExitingHeaderNameAndValue_ReturnsFalse()
    {
        using HttpRequestMessage sut = new();

        Assert.False(sut.HasRequestHeader("Content-Type"));
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("example*")]
    [InlineData("*.com")]
    public void HasRequestHeader_ExistingHeaderNameNotMatchingValue_ReturnsFalse(string value)
    {
        using HttpRequestMessage sut = new();
        sut.Headers.Host = "myhost.net";

        Assert.False(sut.HasRequestHeader("Host", value));
    }
}
