using TestableHttpClient.Response;
using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class RoutingResponseBuilderTests

{
    [Fact]
    public void ByDefault_ReturnsEmptyRoutingResponse()
    {
        RoutingResponseBuilder builder = new();
        RoutingResponse response = builder.RoutingResponse;

        Assert.Empty(response.ResponseMap);
        var fallBackResponse = Assert.IsType<HttpResponse>(response.FallBackResponse);
        Assert.Equal(HttpStatusCode.NotFound, fallBackResponse.StatusCode);
    }

    [Fact]
    public void MapFallBackResponse_SetFallBackResponseCorrectly()
    {
        IResponse fallBackResponse = new TimeoutResponse();
        RoutingResponseBuilder builder = new();

        builder.MapFallBackResponse(fallBackResponse);

        Assert.Same(fallBackResponse, builder.RoutingResponse.FallBackResponse);
    }

    [Fact]
    public void Map_MapsResponseToRouteDefinition()
    {
        IResponse response = new TimeoutResponse();
        RoutingResponseBuilder builder = new();

        builder.Map("https://httpbin.org", response);

        var routeDefintion = builder.RoutingResponse.ResponseMap.Keys.Single();
        Assert.Equal(Value.Exact("https"), routeDefintion.Scheme);
        Assert.Equal(Value.Exact("httpbin.org"), routeDefintion.Host);
        Assert.Equal(Value.Any(), routeDefintion.Path);

        var configuredResponse = builder.RoutingResponse.ResponseMap.Values.Single();
        Assert.Same(response, configuredResponse);
    }
}
