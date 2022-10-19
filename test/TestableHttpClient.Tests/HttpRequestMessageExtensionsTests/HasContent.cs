namespace TestableHttpClient.Tests;

[Obsolete("Testing obsolete methods")]
public partial class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void HasContent_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasContent());
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasContent_NullContent_ReturnsFalse()
    {
        using HttpRequestMessage sut = new();

        Assert.False(sut.HasContent());
    }

    [Fact]
    public void HasContent_NotNullContent_ReturnsTrue()
    {
        using HttpRequestMessage sut = new()
        {
            Content = new StringContent("")
        };

        Assert.True(sut.HasContent());
    }

    [Fact]
    public void HasContent_DisposedContent_ReturnsTrue()
    {
        StringContent content = new("");
        content.Dispose();

        using HttpRequestMessage sut = new()
        {
            Content = content
        };

        Assert.True(sut.HasContent());
    }
}
