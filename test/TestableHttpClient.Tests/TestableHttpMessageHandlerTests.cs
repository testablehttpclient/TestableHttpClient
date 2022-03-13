namespace TestableHttpClient.Tests;

public class TestableHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_WhenRequestsAreMade_LogsRequests()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");

        _ = await client.SendAsync(request);

        Assert.Contains(request, sut.Requests);
    }

    [Fact]
    public async Task SendAsync_WhenMultipleRequestsAreMade_AllRequestsAreLogged()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);
        using var request1 = new HttpRequestMessage(HttpMethod.Get, "https://example1.com/");
        using var request2 = new HttpRequestMessage(HttpMethod.Post, "https://example2.com/");
        using var request3 = new HttpRequestMessage(HttpMethod.Delete, "https://example3.com/");
        using var request4 = new HttpRequestMessage(HttpMethod.Head, "https://example4.com/");

        _ = await client.SendAsync(request1);
        _ = await client.SendAsync(request2);
        _ = await client.SendAsync(request3);
        _ = await client.SendAsync(request4);

        Assert.Equal(new[] { request1, request2, request3, request4 }, sut.Requests);
    }

    [Fact]
    public async Task SendAsync_ByDefault_ReturnsHttpStatusCodeOK()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        var result = await client.GetAsync(new Uri("https://example.com/"));

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
        Assert.NotNull(result.RequestMessage);
    }

    [Fact]
    public async Task SendAsync_ByDefault_ReturnsDifferentResponseForEveryRequest()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        var result1 = await client.GetAsync(new Uri("https://example.com/"));
        var result2 = await client.GetAsync(new Uri("https://example.com/"));

        Assert.NotSame(result1, result2);
    }

    [Fact]
    public async Task SendAsync_ByDefault_SetsRequestMessageOnEveryResponse()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        using var request1 = new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/1"));
        using var request2 = new HttpRequestMessage(HttpMethod.Post, new Uri("https://example.com/2"));

        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        Assert.Same(request1, response1.RequestMessage);
        Assert.Same(request2, response2.RequestMessage);
        Assert.NotSame(response1.RequestMessage, response2.RequestMessage);
    }

#nullable disable
    [Fact]
    public async Task RespondWith_NullFactory_UsesDefaultResponseFactory()
    {
        using var sut = new TestableHttpMessageHandler();
        Func<HttpRequestMessage, HttpResponseMessage> responseFactory = null;
        sut.RespondWith(responseFactory);

        using var client = new HttpClient(sut);
        var response = await client.GetAsync("https://example.com");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(string.Empty, await response.Content.ReadAsStringAsync());
        Assert.NotNull(response.RequestMessage);
    }
#nullable restore

    [Fact]
    public async Task RespondWith_CustomFactory_ReturnsCustomStatusCode()
    {
        using var sut = new TestableHttpMessageHandler();
        static HttpResponseMessage CustomResponse(HttpRequestMessage request) => new HttpResponseMessage(HttpStatusCode.Unauthorized);
        sut.RespondWith(CustomResponse);

        using var client = new HttpClient(sut);
        var response = await client.GetAsync("https://example.com");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Null(response.RequestMessage);
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
}
