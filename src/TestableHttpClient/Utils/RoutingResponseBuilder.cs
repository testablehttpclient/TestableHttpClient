using TestableHttpClient.Response;

namespace TestableHttpClient.Utils;

internal class RoutingResponseBuilder : IRoutingResponseBuilder
{
    public RoutingResponse RoutingResponse { get; } = new();

    public void MapFallBackResponse(IResponse fallBackResponse) => RoutingResponse.FallBackResponse = fallBackResponse;
    public void Map(string route, IResponse response) => RoutingResponse.ResponseMap.Add(UriPatternParser.Parse(route), response);
}
