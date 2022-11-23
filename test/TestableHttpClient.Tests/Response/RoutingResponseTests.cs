using System.Threading;

using TestableHttpClient.Response;
using TestableHttpClient.Utils;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.Tests.Response;

public class RoutingResponseTests
{
    [Fact]
    public async Task ExecuteAsync_NoConfiguredRoutes_Returns404NotFound()
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://httpbin.org/notexisting");
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);

        RoutingResponse response = new();

        await response.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ConfiguredFallBack_ReturnsCorrectFallBackResponse()
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://httpbin.org/notexisting");
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);

        RoutingResponse response = new()
        {
            FallBackResponse = StatusCode(HttpStatusCode.OK)
        };

        await response.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Theory]
    [InlineData("https://httpbin.org/notexisting")]
    [InlineData("https://httpbin.org/get")]
    [InlineData("https://httpbin.org/post")]
    public async Task ExecuteAsync_ConfiguredCatchAllRoute_ReturnsResponseForEveryRequest(string requestUri)
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, requestUri);
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);

        RoutingResponse response = new()
        {
            ResponseMap = new()
            {
                [RouteDefinition.Any] = StatusCode(HttpStatusCode.OK)
            }
        };

        await response.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ConfiguredExactPathMatch_ReturnsResponseForExactMatch()
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://httpbin.org/get");
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);

        RoutingResponse response = new()
        {
            ResponseMap = new()
            {
                [RouteParser.Parse("/get")] = StatusCode(HttpStatusCode.OK)
            }
        };

        await response.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Theory]
    [InlineData("https://httpbin.org/post")]
    [InlineData("https://httpbin.org/getsomething")]
    public async Task ExecuteAsync_ConfiguredExactPathMatch_ReturnsFallBackResponseForNoMatch(string requestUri)
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, requestUri);
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);

        RoutingResponse response = new()
        {
            ResponseMap = new()
            {
                [RouteParser.Parse("/get")] = StatusCode(HttpStatusCode.OK)
            }
        };

        await response.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ConfiguredPartialWildcard_ReturnsResponseForMatch()
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, "https://httpbin.org/delay/100");
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);

        RoutingResponse response = new()
        {
            ResponseMap = new()
            {
                [RouteParser.Parse("/delay/*")] = StatusCode(HttpStatusCode.OK)
            }
        };

        await response.ExecuteAsync(context, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }
}
