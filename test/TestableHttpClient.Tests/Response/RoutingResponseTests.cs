using TestableHttpClient.Response;
using TestableHttpClient.Utils;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.Tests.Response;

public class RoutingResponseTests
{
    [Fact]
    public async Task ExecuteAsync_NoConfiguredRoutes_Returns404NotFound()
    {
        RoutingResponse sut = new();

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ConfiguredFallBack_ReturnsCorrectFallBackResponse()
    {
        RoutingResponse sut = new()
        {
            FallBackResponse = StatusCode(HttpStatusCode.OK)
        };

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Theory]
    [InlineData("https://httpbin.org/notexisting")]
    [InlineData("https://httpbin.org/get")]
    [InlineData("https://httpbin.org/post")]
    public async Task ExecuteAsync_ConfiguredCatchAllRoute_ReturnsResponseForEveryRequest(string requestUri)
    {
        RoutingResponse sut = new()
        {
            ResponseMap = new()
            {
                [UriPattern.Any] = StatusCode(HttpStatusCode.OK)
            }
        };

        using HttpResponseMessage responseMessage = await sut.TestAsync(requestUri);

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ConfiguredExactPathMatch_ReturnsResponseForExactMatch()
    {
        RoutingResponse sut = new()
        {
            ResponseMap = new()
            {
                [UriPatternParser.Parse("/get")] = StatusCode(HttpStatusCode.OK)
            }
        };

        using HttpResponseMessage responseMessage = await sut.TestAsync("https://httpbin.org/get");

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    [Theory]
    [InlineData("https://httpbin.org/post")]
    [InlineData("https://httpbin.org/getsomething")]
    public async Task ExecuteAsync_ConfiguredExactPathMatch_ReturnsFallBackResponseForNoMatch(string requestUri)
    {
        RoutingResponse sut = new()
        {
            ResponseMap = new()
            {
                [UriPatternParser.Parse("/get")] = StatusCode(HttpStatusCode.OK)
            }
        };

        using HttpResponseMessage responseMessage = await sut.TestAsync(requestUri);

        Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
    }

    [Fact]
    public async Task ExecuteAsync_ConfiguredPartialWildcard_ReturnsResponseForMatch()
    {
        RoutingResponse sut = new()
        {
            ResponseMap = new()
            {
                [UriPatternParser.Parse("/delay/*")] = StatusCode(HttpStatusCode.OK)
            }
        };

        using HttpResponseMessage responseMessage = await sut.TestAsync("https://httpbin.org/delay/100");

        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }
}
