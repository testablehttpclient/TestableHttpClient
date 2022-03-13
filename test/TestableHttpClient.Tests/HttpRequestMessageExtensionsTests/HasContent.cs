namespace TestableHttpClient.Tests;

public partial class HttpRequestMessageExtensionsTests
{
#nullable disable
    [Fact]
    public void HasContent_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContent());
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }
#nullable restore

    [Fact]
    public void HasContent_NullContent_ReturnsFalse()
    {
        using var sut = new HttpRequestMessage();

        Assert.False(sut.HasContent());
    }

    [Fact]
    public void HasContent_NotNullContent_ReturnsTrue()
    {
        using var sut = new HttpRequestMessage
        {
            Content = new StringContent("")
        };

        Assert.True(sut.HasContent());
    }
}
