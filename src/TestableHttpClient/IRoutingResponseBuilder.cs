namespace TestableHttpClient;

public interface IRoutingResponseBuilder
{
    void Map(string route, IResponse response);
    void MapFallBackResponse(IResponse fallBackResponse);
}
