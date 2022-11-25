using static System.Net.HttpStatusCode;
using static TestableHttpClient.Responses;

namespace TestableHttpClient.Response;
internal class RoutingResponse : IResponse
{
    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        IResponse response = FallBackResponse;

        if (context.HttpRequestMessage.RequestUri is not null)
        {
            response = GetResponseForRequest(context.HttpRequestMessage.RequestUri, context.Options.RoutingOptions);
        }

        return response.ExecuteAsync(context, cancellationToken);
    }

    public Dictionary<RouteDefinition, IResponse> ResponseMap { get; init; } = new();
    public IResponse FallBackResponse { get; internal set; } = StatusCode(NotFound);

    private IResponse GetResponseForRequest(Uri requestUri, RoutingOptions routingOptions)
    {
        var matchingResponse = ResponseMap.FirstOrDefault(x => x.Key.Matches(requestUri, routingOptions));

        return matchingResponse.Value switch
        {
            null => FallBackResponse,
            _ => matchingResponse.Value
        };
    }
}
