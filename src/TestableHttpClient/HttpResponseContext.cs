namespace TestableHttpClient;

/// <summary>
/// This class contains contextual information for generating responses.
/// </summary>
public class HttpResponseContext
{
    public HttpResponseContext(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage, TestableHttpMessageHandlerOptions? options = null)
    {
        HttpRequestMessage = httpRequestMessage;
        HttpResponseMessage = httpResponseMessage;
        Options = options ?? new TestableHttpMessageHandlerOptions();
    }

    /// <summary>
    /// The request message that is send by the HttpClient.
    /// </summary>
    public HttpRequestMessage HttpRequestMessage { get; }
    /// <summary>
    /// The response message that will be send back to the HttpClient.
    /// </summary>
    public HttpResponseMessage HttpResponseMessage { get; }
    public TestableHttpMessageHandlerOptions Options { get; }
}
