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
    public async Task TextResponse_WithEmptyString_SetsStringToContent()
    {
        TextResponse sut = new(string.Empty);

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal(string.Empty, sut.Content);
        Assert.Equal(string.Empty, await responseMessage.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task TextResponse_WithNotString_SetsStringToContent()
    {
        TextResponse sut = new("Hello World");

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal("Hello World", sut.Content);
        Assert.Equal("Hello World", await responseMessage.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomEncoding_SetsEncoding()
    {
        TextResponse sut = new("", Encoding.ASCII, null);

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal(Encoding.ASCII, sut.Encoding);
        Assert.Equal("text/plain", sut.MediaType);
        Assert.Equal("us-ascii", responseMessage.Content.Headers.ContentType?.CharSet);
        Assert.Equal("text/plain", responseMessage.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomMediaType_SetsMediaType()
    {
        TextResponse sut = new("", null, "text/xml");

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal(Encoding.UTF8, sut.Encoding);
        Assert.Equal("text/xml", sut.MediaType);
        Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType?.CharSet);
        Assert.Equal("text/xml", responseMessage.Content.Headers.ContentType?.MediaType);
    }
}
