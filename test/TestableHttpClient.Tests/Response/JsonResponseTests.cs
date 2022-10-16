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
    public async Task ExecuteAsync_WithOutCustomContentType_SetsMediaType()
    {
        JsonResponse sut = new(null, null);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal("application/json", sut.ContentType);
        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomContentType_SetsMediaType()
    {
        JsonResponse sut = new(null, "application/problem+json");
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal("application/problem+json", sut.ContentType);
        Assert.Equal("application/problem+json", responseMessage.Content.Headers.ContentType?.MediaType);
    }
}
