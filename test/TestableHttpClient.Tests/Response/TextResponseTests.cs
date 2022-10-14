using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class TextResponseTests
{
    [Fact]
    public void TextResponse_WithNullString_ThrowsNewArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new TextResponse(null!));
        Assert.Equal("content", exception.ParamName);
    }

    [Fact]
    public async Task TextResponse_WithEmtpyString_SetsStringToContent()
    {
        TextResponse sut = new(string.Empty);
        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Equal(string.Empty, sut.Content);
        Assert.Equal(string.Empty, await response.Content.ReadAsStringAsync());
        Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task TextResponse_WithNotString_SetsStringToContent()
    {
        TextResponse sut = new("Hello World");
        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Equal("Hello World", sut.Content);
        Assert.Equal("Hello World", await response.Content.ReadAsStringAsync());
        Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
    }
}
