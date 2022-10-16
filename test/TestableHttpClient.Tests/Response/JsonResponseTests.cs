using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class JsonResponseTests
{
    [Theory]
    [InlineData(null, "null")]
    [InlineData("hello", "\"hello\"")]
    [InlineData(42, "42")]
    public async Task GetResponseAsync_WithSimpleContent_SetsJsonStringToContent(object? input, string expectedJson)
    {
        JsonResponse sut = new(input);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal(expectedJson, json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task GetResponseAsync_WithObjectContent_SetsJsonStringToContent()
    {
        var input = new { Value = 42 };
        JsonResponse sut = new(input);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("{\"Value\":42}", json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task GetResponseAsync_WithCollectionContent_SetsJsonStringToContent()
    {
        var input = new[] { 1, 2, 3, 4 };
        JsonResponse sut = new(input);
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("[1,2,3,4]", json);
        Assert.Same(input, sut.Content);
    }
}
