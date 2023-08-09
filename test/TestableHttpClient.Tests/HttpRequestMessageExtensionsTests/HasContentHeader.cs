namespace TestableHttpClient.Tests;

public partial class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void HasContentHeader_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContentHeader("Content-Disposition"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void HasContentHeader_NullHeaderName_ThrowsArgumentNullException(string headerName)
    {
        using HttpRequestMessage sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContentHeader(headerName));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void HasContentHeader_NullRequestNonNullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContentHeader("Content-Disposition", "inline"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void HasContentHeader_NullHeaderNameAndNonNullHeaderValue_ThrowsArgumentNullException(string headerName)
    {
        using HttpRequestMessage sut = new();
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContentHeader(headerName, "inline"));
        Assert.Equal("headerName", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void HasContentHeader_NonNullHeaderNameAndNullHeaderValue_ThrowsArgumentNullException(string headerValue)
    {
        using HttpRequestMessage sut = new();
        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContentHeader("Content-Disposition", headerValue));
        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void HasContentHeader_ExistingHeaderName_ReturnsTrue()
    {
        using HttpRequestMessage sut = new()
        {
            Content = new StringContent("")
        };
        sut.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");

        Assert.True(sut.HasContentHeader("Content-Disposition"));
    }

    [Theory]
    [InlineData("Host")]
    [InlineData("Content-Disposition")]
    public void HasContentHeader_NotExistingHeaderName_ReturnsFalse(string headerName)
    {
        using HttpRequestMessage sut = new()
        {
            Content = new StringContent("")
        };

        Assert.False(sut.HasContentHeader(headerName));
    }

    [Fact]
    public void HasContentHeader_NoContent_ReturnsFalse()
    {
        using HttpRequestMessage sut = new()
        {
            Content = null
        };

        Assert.False(sut.HasContentHeader("Content-Disposition"));
    }

    [Theory]
    [InlineData("inline; filename=empty.file")]
    [InlineData("inline; *")]
    [InlineData("*; filename=empty.file")]
    [InlineData("*")]
    public void HasContentHeader_ExistingHeaderNameMatchingValue_ReturnsTrue(string value)
    {
        using HttpRequestMessage sut = new()
        {
            Content = new StringContent("")
        };
        sut.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
        {
            FileName = "empty.file"
        };

        Assert.True(sut.HasContentHeader("Content-Disposition", value));
    }

    [Fact]
    public void HasContentHeader_NotExitingHeaderNameAndValue_ReturnsFalse()
    {
        using HttpRequestMessage sut = new()
        {
            Content = new StringContent("")
        };

        Assert.False(sut.HasContentHeader("Host", "inline"));
    }

    [Fact]
    public void HasContentHeader_NullContentNotExitingHeaderNameAndValue_ReturnsFalse()
    {
        using HttpRequestMessage sut = new()
        {
            Content = null
        };

        Assert.False(sut.HasContentHeader("Host", "inline"));
    }

    [Theory]
    [InlineData("inline; filename=emtpy.file")]
    [InlineData("inline; *")]
    [InlineData("*; filename=empty.file")]
    public void HasContentHeader_ExistingHeaderNameNotMatchingValue_ReturnsFalse(string value)
    {
        using HttpRequestMessage sut = new()
        {
            Content = new StringContent("")
        };
        sut.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = "attachment.file"
        };

        Assert.False(sut.HasContentHeader("Content-Disposition", value));
    }
}
