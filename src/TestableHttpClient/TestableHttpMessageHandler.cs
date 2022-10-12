using System.Collections.Concurrent;

using TestableHttpClient.Response;

namespace TestableHttpClient;
/// <summary>
/// A testable HTTP message handler that captures all requests and always returns the same response.
/// </summary>
public class TestableHttpMessageHandler : HttpMessageHandler
{
    private readonly ConcurrentQueue<HttpRequestMessage> httpRequestMessages = new ConcurrentQueue<HttpRequestMessage>();
    private IResponse response = new StatusCodeResponse(HttpStatusCode.OK);

    /// <summary>
    /// Gets the collection of captured requests made using this HttpMessageHandler.
    /// </summary>
    public IEnumerable<HttpRequestMessage> Requests => httpRequestMessages;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        httpRequestMessages.Enqueue(request);

        HttpResponseMessage responseMessage = await response.GetResponseAsync(request, cancellationToken);
        if (responseMessage.RequestMessage is null)
        {
            responseMessage.RequestMessage = request;
        }
        if (responseMessage.Content is null)
        {
            responseMessage.Content = new StringContent("");
        }

        if (responseMessage is TimeoutHttpResponseMessage)
        {
            var cancelationSource = cancellationToken.GetSource();

            if (cancelationSource is not null)
            {
                cancelationSource.Cancel(false);
            }
            return await Task.FromCanceled<HttpResponseMessage>(cancellationToken);
        }

        return responseMessage;
    }

    public void RespondWith(IResponse response)
    {
        this.response = response;
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
        if (httpResponseMessageFactory is null)
        {
            throw new ArgumentNullException(nameof(httpResponseMessageFactory));
        }

        RespondWith(new FunctionResponse(httpResponseMessageFactory));
    }
}
