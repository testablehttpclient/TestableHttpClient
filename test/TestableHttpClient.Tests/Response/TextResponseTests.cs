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
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);
        await sut.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal(string.Empty, sut.Content);
        Assert.Equal(string.Empty, await responseMessage.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task TextResponse_WithNotString_SetsStringToContent()
    {
        TextResponse sut = new("Hello World");
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);
        await sut.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal("Hello World", sut.Content);
        Assert.Equal("Hello World", await responseMessage.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomEncoding_SetsEncoding()
    {
        TextResponse sut = new("", Encoding.ASCII, null);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal(Encoding.ASCII, sut.Encoding);
        Assert.Equal("text/plain", sut.MediaType);
        Assert.Equal("us-ascii", responseMessage.Content.Headers.ContentType?.CharSet);
        Assert.Equal("text/plain", responseMessage.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomMediaType_SetsMediaType()
    {
        TextResponse sut = new("", null, "text/xml");
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal(Encoding.UTF8, sut.Encoding);
        Assert.Equal("text/xml", sut.MediaType);
        Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType?.CharSet);
        Assert.Equal("text/xml", responseMessage.Content.Headers.ContentType?.MediaType);
    }
}
