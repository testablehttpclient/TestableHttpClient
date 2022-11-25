using System;
using System.Net.Http;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.IntegrationTests;

public class ConfigureResponses
{
    [Fact]
    public async Task UsingTestHandler_WithoutSettingUpResponse_Returns200OKWithoutContent()
    {
        using var testHandler = new TestableHttpMessageHandler();

        using var httpClient = new HttpClient(testHandler);
        var result = await httpClient.GetAsync("http://httpbin.org/status/200");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task UsingTestHandlerWithCustomResponse_ReturnsCustomResponse()
    {
        using var testHandler = new TestableHttpMessageHandler();
        testHandler.RespondWith(Text("HttpClient testing is easy"));

        using var httpClient = new HttpClient(testHandler);
        var result = await httpClient.GetAsync("http://httpbin.org/status/200");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
    }

    [Fact]
    [Obsolete("Use ConfiguredResponse or a custom response instead.")]
    public async Task UsingTestHandlerWithCustomResponseUsingBuilder_ReturnsCustomResponse()
    {
        using var testHandler = new TestableHttpMessageHandler();
        testHandler.RespondWith(response =>
        {
            response.WithHttpStatusCode(HttpStatusCode.Created)
                    .WithStringContent("HttpClient testing is easy");
        });

        using var httpClient = new HttpClient(testHandler);
        var result = await httpClient.GetAsync("http://httpbin.org/status/201");

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task UsingTestHandlerWithMultipleCustomRepsonse_ReturnsLastCustomResponse()
    {
        using var testHandler = new TestableHttpMessageHandler();
        testHandler.RespondWith(Text("HttpClient testing is easy"));
        testHandler.RespondWith(Json("Not Found", HttpStatusCode.NotFound));

        using var httpClient = new HttpClient(testHandler);
        var result = await httpClient.GetAsync("http://httpbin.org/status/201");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Equal("\"Not Found\"", await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task UsingTestHandlerWithCustomResponse_AlwaysReturnsSameCustomResponse()
    {
        using var testHandler = new TestableHttpMessageHandler();
        testHandler.RespondWith(Text("HttpClient testing is easy"));

        using var httpClient = new HttpClient(testHandler);
        var urls = new[]
        {
            "http://httpbin.org/status/200",
            "http://httpbin.org/status/201",
            "http://httpbin.org/status/400",
            "http://httpbin.org/status/401",
            "http://httpbin.org/status/503",
        };

        foreach (var url in urls)
        {
            var result = await httpClient.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
        }
    }

    [Fact]
    public async Task UsingTestHandlerWithCustomResponseFactory_AllowsForAdvancedUsecases()
    {
        using var testHandler = new TestableHttpMessageHandler();

        static IResponse PathBasedResponse(HttpResponseContext context) => context.HttpRequestMessage switch
        {
            _ when context.HttpRequestMessage.RequestUri?.AbsolutePath == "/status/200" => StatusCode(HttpStatusCode.OK),
            _ when context.HttpRequestMessage.RequestUri?.AbsolutePath == "/status/400" => StatusCode(HttpStatusCode.BadRequest),
            _ => StatusCode(HttpStatusCode.NotFound)
        };
        testHandler.RespondWith(SelectResponse(PathBasedResponse));

        using var httpClient = new HttpClient(testHandler);
        var response = await httpClient.GetAsync("http://httpbin/status/200");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await httpClient.GetAsync("http://httpbin.org/status/400");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        response = await httpClient.GetAsync("http://httpbin.org/status/500");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UsingTestHandlerWithRoute_AllowsForRoutingUsecases()
    {
        using var testHandler = new TestableHttpMessageHandler();

        testHandler.RespondWith(Route(builder =>
        {
            builder.Map("http://*", StatusCode(HttpStatusCode.Redirect));
            builder.Map("/status/200", StatusCode(HttpStatusCode.OK));
            builder.Map("/status/400", StatusCode(HttpStatusCode.BadRequest));
        }));

        using var httpClient = new HttpClient(testHandler);
        var response = await httpClient.GetAsync("http://httpbin/status/200");
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
        using var testHandler = new TestableHttpMessageHandler();
        testHandler.RespondWith(Timeout());

        using var httpClient = new HttpClient(testHandler);
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
        var response = await httpClient.GetAsync("http://httpbin.org/anything");
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
        var response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task YsingTestHandlerWithConfiguredResponses_WillConfigureTHeResponse()
    {
        using TestableHttpMessageHandler testHandler = new();
        testHandler.RespondWith(Configured(StatusCode(HttpStatusCode.NoContent), x => x.Headers.Add("server", "test")));

        using HttpClient httpClient = new(testHandler);
        var response = await httpClient.GetAsync("http://httpbin.org/anything");
        Assert.Equal("test", response.Headers.Server.ToString());
    }
}
