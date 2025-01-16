using static System.Net.HttpStatusCode;
using static TestableHttpClient.Responses;

namespace TestableHttpClient.Response;
internal sealed class RoutingResponse : IResponse
{
    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        IResponse response = FallBackResponse;

        if (context.HttpRequestMessage.RequestUri is not null)
        {
            response = GetResponseForRequest(context.HttpRequestMessage.RequestUri, context.Options.UriPatternMatchingOptions);
        }

        return response.ExecuteAsync(context, cancellationToken);
    }

    public Dictionary<UriPattern, IResponse> ResponseMap { get; init; } = new();
    public IResponse FallBackResponse { get; internal set; } = StatusCode(NotFound);

    private IResponse GetResponseForRequest(Uri requestUri, UriPatternMatchingOptions uriPatternOptions)
    {
        var matchingResponse = ResponseMap.FirstOrDefault(x => x.Key.Matches(requestUri, uriPatternOptions));

        return matchingResponse.Value switch
        {
            null => FallBackResponse,
            _ => matchingResponse.Value
        };
    }
}
