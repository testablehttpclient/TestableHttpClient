namespace TestableHttpClient.Tests;

public partial class HttpResponseMessageExtensionsTests
{
#nullable disable
    [Fact]
    public void HasContentWithPattern_NullResponse_ThrowsArgumentNullException()
    {
        HttpResponseMessage sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContent(""));
        Assert.Equal("httpResponseMessage", exception.ParamName);
    }

    [Fact]
    public void HasContentWithPattern_NullExpectedContent_ThrowsArgumentNullException()
    {
        using var sut = new HttpResponseMessage();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContent(null));
        Assert.Equal("pattern", exception.ParamName);
    }
#nullable enable

    [Fact]
    public void HasContentWithPattern_NoContentAndEmptyPattern_ReturnsTrue()
    {
        using var sut = new HttpResponseMessage();

        Assert.True(sut.HasContent(""));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void HasContentWithPattern_ExactlyMatchingStringContent_ReturnsTrue(string content)
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent(content)
        };

        Assert.True(sut.HasContent(content));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void HasContentWithPattern_NotMatchingStringContent_ReturnsFalse(string content)
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent("Example content")
        };

        Assert.False(sut.HasContent(content));
    }

    [Theory]
    [InlineData("*")]
    [InlineData("username=*&password=*")]
    [InlineData("*admin*")]
    public void HasContentWithPattern_MatchingPattern_ReturnsTrue(string pattern)
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent("username=admin&password=admin")
        };

        Assert.True(sut.HasContent(pattern));
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("*test*")]
    public void HasContentWithPattern_NotMatchingPattern_ReturnsFalse(string pattern)
    {
        using var sut = new HttpResponseMessage
        {
            Content = new StringContent("username=admin&password=admin")
        };

        Assert.False(sut.HasContent(pattern));
    }
}
