namespace TestableHttpClient;

/// <summary>
/// A builder for routing responses.
/// </summary>
public interface IRoutingResponseBuilder
{
    /// <summary>
    /// Maps a route to a specified response.
    /// </summary>
    /// <param name="route">The route pattern.</param>
    /// <param name="response">The response the route should return.</param>
    /// <example>x.Map("*", Responses.StatusCode(HttpStatusCode.OK))</example>
    public void Map(string route, IResponse response);
    /// <summary>
    /// Maps a custom response for when a request didn't match any route. Defaults to Responses.StatusCode(HttpStatusCode.NotFound).
    /// </summary>
    /// <param name="fallBackResponse">The response that should be returned when no route matches.</param>
    public void MapFallBackResponse(IResponse fallBackResponse);
}
