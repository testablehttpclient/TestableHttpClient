using TestableHttpClient.Response;

namespace TestableHttpClient.Utils;

internal sealed class RoutingResponseBuilder : IRoutingResponseBuilder
{
    public RoutingResponse RoutingResponse { get; } = new();

    public IRoutingResponseBuilder MapFallBackResponse(IResponse fallBackResponse)
    {
        RoutingResponse.FallBackResponse = fallBackResponse;
        return this;
    }

    public IRoutingResponseBuilder Map(string route, IResponse response)
    {
        RoutingResponse.ResponseMap.Add(UriPatternParser.Parse(route), response);
        return this;
    }
}
