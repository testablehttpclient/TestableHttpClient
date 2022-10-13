namespace TestableHttpClient.Tests.TestableHttpMessageHandlerResponseExtensionsTests;

public class RespondWithResponseFactoryTests
{
    [Fact]
    public void RespondWith_NullHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(builder => builder.WithHttpStatusCode(HttpStatusCode.BadRequest)));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void RespondWith_NullFactory_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();
        Func<HttpRequestMessage, HttpResponseMessage> responseBuilder = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(responseBuilder));
        Assert.Equal("httpResponseMessageFactory", exception.ParamName);
    }

    [Fact]
    public async Task RespondWith_CustomFactory_ReturnsCustomStatusCode()
    {
        using var sut = new TestableHttpMessageHandler();
        static HttpResponseMessage CustomResponse(HttpRequestMessage request) => new HttpResponseMessage(HttpStatusCode.Unauthorized);
        sut.RespondWith(CustomResponse);

        using var client = new HttpClient(sut);
        var response = await client.GetAsync("https://example.com");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.NotNull(response.RequestMessage);
    }

    [Fact]
    public async Task RespondWith_CustomFactory_FactoryIsCalledEveryTimeARequestIsMade()
    {
        var responseFactoryCallCount = 0;
        using var sut = new TestableHttpMessageHandler();
        HttpResponseMessage CustomResponse(HttpRequestMessage request)
        {
            responseFactoryCallCount++;
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
        sut.RespondWith(CustomResponse);

        using var client = new HttpClient(sut);
        _ = await client.GetAsync("https://example.com");
        _ = await client.GetAsync("https://example.com");
        _ = await client.GetAsync("https://example.com");
        _ = await client.GetAsync("https://example.com");
        _ = await client.GetAsync("https://example.com");

        Assert.Equal(5, responseFactoryCallCount);
    }

    [Fact]
    public async Task RespondWith_UsingResponseBuilder_SetRequestMessageIsNotOverwritten()
    {
        using HttpRequestMessage requestMessage = new();
        using var sut = new TestableHttpMessageHandler();
        HttpResponseMessage CustomResponse(HttpRequestMessage request)
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = requestMessage
            };
        }
        sut.RespondWith(CustomResponse);


        using var client = new HttpClient(sut);

        var response = await client.GetAsync(new Uri("https://example.com"));

        Assert.Same(requestMessage, response.RequestMessage);
    }
}
