using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class JsonResponseTests
{
    [Theory]
    [InlineData(null, "null")]
    [InlineData("hello", "\"hello\"")]
    [InlineData(42, "42")]
    public async Task ExecuteAsync_WithSimpleContent_SetsJsonStringToContent(object? input, string expectedJson)
    {
        JsonResponse sut = new(input);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal(expectedJson, json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithObjectContent_SetsJsonStringToContent()
    {
        var input = new { Value = 42 };
        JsonResponse sut = new(input);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("{\"Value\":42}", json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithCollectionContent_SetsJsonStringToContent()
    {
        var input = new[] { 1, 2, 3, 4 };
        JsonResponse sut = new(input);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("[1,2,3,4]", json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomEncoding_SetsEncoding()
    {
        JsonResponse sut = new(null, Encoding.ASCII, null);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal(Encoding.ASCII, sut.Encoding);
        Assert.Equal("application/json", sut.MediaType);
        Assert.Equal("us-ascii", responseMessage.Content.Headers.ContentType?.CharSet);
        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomMediaType_SetsMediaType()
    {
        JsonResponse sut = new(null, null, "application/problem+json");
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal(Encoding.UTF8, sut.Encoding);
        Assert.Equal("application/problem+json", sut.MediaType);
        Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType?.CharSet);
        Assert.Equal("application/problem+json", responseMessage.Content.Headers.ContentType?.MediaType);
    }
}
