using System.Collections.Concurrent;

namespace TestableHttpClient;
/// <summary>
/// A testable HTTP message handler that captures all requests and always returns the same response.
/// </summary>
public class TestableHttpMessageHandler : HttpMessageHandler
{
    private readonly ConcurrentQueue<HttpRequestMessage> httpRequestMessages = new ConcurrentQueue<HttpRequestMessage>();
    private Func<HttpRequestMessage, HttpResponseMessage> responseFactory = DefaultResponseFactory;

    private static HttpResponseMessage DefaultResponseFactory(HttpRequestMessage requestMessage) => new HttpResponseMessage(HttpStatusCode.OK)
    {
        RequestMessage = requestMessage,
        Content = new StringContent(string.Empty)
    };

    /// <summary>
    /// Gets the collection of captured requests made using this HttpMessageHandler.
    /// </summary>
    public IEnumerable<HttpRequestMessage> Requests => httpRequestMessages;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        httpRequestMessages.Enqueue(request);

        var response = responseFactory(request);

        if (response is TimeoutHttpResponseMessage)
        {
            var cancelationSource = cancellationToken.GetSource();

            if (cancelationSource is not null)
            {
                cancelationSource.Cancel(false);
            }
            throw new TaskCanceledException(new OperationCanceledException().Message);
        }

        return Task.FromResult(response);
    }

    /// <summary>
    /// Configure a factory method that creates a <see cref="HttpResponseMessage"/> that should be returned for a request.
    /// </summary>
    /// <param name="httpResponseMessageFactory">The factory method that should be called for every request. The request is passed as a parameter to the factory method and it is expected to return a HttpResponseMessage.</param>
    /// <remarks>By default each request will receive a new response, however this is dependend on the implementation.</remarks>
    /// <example>
    /// testableHttpMessageHander.RespondWith(request => new ResponseMessage(HttpStatusCode.Unauthorized) { RequestMessage = request };
    /// </example>
    public void RespondWith(Func<HttpRequestMessage, HttpResponseMessage> httpResponseMessageFactory)
    {
        responseFactory = httpResponseMessageFactory ?? DefaultResponseFactory;
    }
}
