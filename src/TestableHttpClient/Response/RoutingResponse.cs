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
            response = GetResponseForRequest(context.HttpRequestMessage.RequestUri);
        }

        return response.ExecuteAsync(context, cancellationToken);
    }

    public Dictionary<RouteDefinition, IResponse> ResponseMap { get; init; } = new();
    public IResponse FallBackResponse { get; init; } = StatusCode(NotFound);

    private IResponse GetResponseForRequest(Uri requestUri)
    {
        foreach (var responsePair in ResponseMap)
        {
            if (responsePair.Key.Matches(requestUri))
            {
                return responsePair.Value;
            }
        }

        return FallBackResponse;
    }
}
