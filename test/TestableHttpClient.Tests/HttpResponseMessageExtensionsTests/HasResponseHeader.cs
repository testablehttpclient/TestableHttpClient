namespace TestableHttpClient.Tests;

public partial class HttpResponseMessageExtensionsTests
{
#nullable disable
    [Fact]
    public void HasResponseHeader_NullResponse_ThrowsArgumentNullException()
    {
        HttpResponseMessage sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasResponseHeader("Server"));
        Assert.Equal("httpResponseMessage", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void HasResponseHeader_NullHeaderName_ThrowsArgumentNullException(string headerName)
    {
        using var sut = new HttpResponseMessage();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasResponseHeader(headerName));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void HasResponseHeader_NullResponseNonNullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException()
    {
        HttpResponseMessage sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasResponseHeader("Server", "value"));
        Assert.Equal("httpResponseMessage", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void HasResponseHeader_NullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException(string headerName)
    {
        using var sut = new HttpResponseMessage();
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasResponseHeader(headerName, "value"));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void HasResponseHeader_NonNullHeaderNameAndNullHeaderValue_ThrowsArgumentNullException(string headerValue)
    {
        using var sut = new HttpResponseMessage();
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasResponseHeader("Server", headerValue));
        Assert.Equal("headerValue", exception.ParamName);
    }
#nullable restore

    [Fact]
    public void HasRequestHeader_ExistingHeaderName_ReturnsTrue()
    {
        using var sut = new HttpResponseMessage();
        sut.Headers.Add("Server", "example server");

        Assert.True(sut.HasResponseHeader("Server"));
    }

    [Theory]
    [InlineData("Host")]
    [InlineData("Content-Type")]
    public void HasRequestHeader_NotExistingHeaderName_ReturnsFalse(string headerName)
    {
        using var sut = new HttpResponseMessage();

        Assert.False(sut.HasResponseHeader(headerName));
    }

    [Theory]
    [InlineData("example server")]
    [InlineData("example*")]
    [InlineData("*server")]
    [InlineData("*")]
    public void HasResponseHeader_ExistingHeaderNameMatchingValue_ReturnsTrue(string value)
    {
        using var sut = new HttpResponseMessage();
        sut.Headers.Add("Server", "example server");

        Assert.True(sut.HasResponseHeader("Server", value));
    }

    [Fact]
    public void HasResponseHeader_NotExitingHeaderNameAndValue_ReturnsFalse()
    {
        using var sut = new HttpResponseMessage();

        Assert.False(sut.HasResponseHeader("Content-Type"));
    }

    [Theory]
    [InlineData("example server")]
    [InlineData("example*")]
    [InlineData("*server")]
    public void HasResponseHeader_ExistingHeaderNameNotMatchingValue_ReturnsFalse(string value)
    {
        using var sut = new HttpResponseMessage();
        sut.Headers.Add("Server", "My Application");

        Assert.False(sut.HasResponseHeader("Server", value));
    }
}
