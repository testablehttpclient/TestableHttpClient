using System.Text.Json;

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

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal(expectedJson, json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithObjectContentAndCustomSettingsFromContext_SetsJsonStringToContent()
    {
        var input = new { Value = 42 };
        JsonResponse sut = new(input);
        using TestableHttpMessageHandler handler = new();
        handler.Options.JsonSerializerOptions.PropertyNamingPolicy = null;
        handler.RespondWith(sut);
        using HttpClient client = new(handler);

        using HttpResponseMessage responseMessage = await client.GetAsync("http://example");

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("{\"Value\":42}", json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithObjectContentAndDefaultSettingsViaContext_SetsJsonStringToContent()
    {
        var input = new { Value = 42 };
        JsonResponse sut = new(input);
        using HttpResponseMessage responseMessage = await sut.TestAsync();

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("{\"value\":42}", json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithObjectContentAndCustomSettingsDirectly_SetsJsonStringToContent()
    {
        var input = new { Value = 42 };
        JsonResponse sut = new(input)
        {
            JsonSerializerOptions = new JsonSerializerOptions()
        };

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("{\"Value\":42}", json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithCollectionContent_SetsJsonStringToContent()
    {
        int[] input = [ 1, 2, 3, 4 ];
        JsonResponse sut = new(input);

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        var json = await responseMessage.Content.ReadAsStringAsync();

        Assert.Equal("[1,2,3,4]", json);
        Assert.Same(input, sut.Content);
    }

    [Fact]
    public async Task ExecuteAsync_WithOutCustomContentType_SetsMediaType()
    {
        JsonResponse sut = new(null, null);

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal("application/json", sut.ContentType);
        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomContentType_SetsMediaType()
    {
        JsonResponse sut = new(null, "application/problem+json");

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal("application/problem+json", sut.ContentType);
        Assert.Equal("application/problem+json", responseMessage.Content.Headers.ContentType?.MediaType);
    }
}
