using static TestableHttpClient.Responses;

namespace TestableHttpClient.IntegrationTests;

public class ConfigureResponses
{
    [Fact]
    public async Task UsingTestHandler_WithoutSettingUpResponse_Returns200OKWithoutContent()
    {
        using TestableHttpMessageHandler testHandler = new();

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage result = await httpClient.GetAsync("http://httpbin.org/status/200");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task UsingTestHandlerWithCustomResponse_ReturnsCustomResponse()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Text("HttpClient testing is easy"));

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage result = await httpClient.GetAsync("http://httpbin.org/status/200");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task UsingTestHandlerWithMultipleCustomResponse_ReturnsLastCustomResponse()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Text("HttpClient testing is easy"));
        testHandler.RespondWith(Json("Not Found", HttpStatusCode.NotFound));

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage result = await httpClient.GetAsync("http://httpbin.org/status/201");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Equal("\"Not Found\"", await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task UsingTestHandlerWithCustomResponse_AlwaysReturnsSameCustomResponse()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Text("HttpClient testing is easy"));

        using HttpClient httpClient = new(testHandler);
        string[] urls = [
            "http://httpbin.org/status/200",
            "http://httpbin.org/status/201",
            "http://httpbin.org/status/400",
            "http://httpbin.org/status/401",
            "http://httpbin.org/status/503",
        ];

        foreach (string? url in urls)
        {
            HttpResponseMessage result = await httpClient.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
        }
    }

    [Fact]
    public async Task UsingTestHandlerWithCustomResponseFactory_AllowsForAdvancedUseCases()
    {
        using TestableHttpMessageHandler testHandler = new();

        static IResponse PathBasedResponse(HttpResponseContext context)
        {
            return context.HttpRequestMessage switch
            {
                _ when context.HttpRequestMessage.RequestUri?.AbsolutePath == "/status/200" => StatusCode(HttpStatusCode.OK),
                _ when context.HttpRequestMessage.RequestUri?.AbsolutePath == "/status/400" => StatusCode(HttpStatusCode.BadRequest),
                _ => StatusCode(HttpStatusCode.NotFound)
            };
        }

        testHandler.RespondWith(SelectResponse(PathBasedResponse));

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage response = await httpClient.GetAsync("http://httpbin/status/200");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await httpClient.GetAsync("http://httpbin.org/status/400");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        response = await httpClient.GetAsync("http://httpbin.org/status/500");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UsingTestHandlerWithRoute_AllowsForRoutingUseCases()
    {
        using TestableHttpMessageHandler testHandler = new();

        testHandler.RespondWith(Route(builder =>
        {
            builder.Map("http://*", StatusCode(HttpStatusCode.Redirect));
            builder.Map("/status/200", StatusCode(HttpStatusCode.OK));
            builder.Map("/status/400", StatusCode(HttpStatusCode.BadRequest));
        }));

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage response = await httpClient.GetAsync("http://httpbin/status/200");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        response = await httpClient.GetAsync("https://httpbin/status/200");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await httpClient.GetAsync("https://httpbin.org/status/400");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        response = await httpClient.GetAsync("https://httpbin.org/status/500");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task SimulateTimeout_WillThrowExceptionSimulatingTheTimeout()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Timeout());

        using HttpClient httpClient = new(testHandler);
        await Assert.ThrowsAsync<TaskCanceledException>(() => httpClient.GetAsync("https://httpbin.org/delay/500"));
    }

    [Fact]
    public async Task UsingTestHandlerWithSequencedResponses_WillReturnDifferentResponseForEveryRequest()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Sequenced(
            StatusCode(HttpStatusCode.OK),
            StatusCode(HttpStatusCode.Unauthorized),
            StatusCode(HttpStatusCode.NoContent),
            StatusCode(HttpStatusCode.NotFound)
            ));

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        // Last configured response is returned when all other responses are used.
        response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UsingTestHandlerWithDelayedResponses_WillDelayTheResponse()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Delayed(StatusCode(HttpStatusCode.OK), TimeSpan.FromSeconds(1)));

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UsingTestHandlerWithConfiguredResponses_WillConfigureTheResponse()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Configured(StatusCode(HttpStatusCode.NoContent), x => x.Headers.Add("server", "test")));

        using HttpClient httpClient = new(testHandler);
        HttpResponseMessage response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal("test", response.Headers.Server.ToString());
    }
}
