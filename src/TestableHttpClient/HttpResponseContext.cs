namespace TestableHttpClient;

/// <summary>
/// This class contains contextual information for generating responses.
/// </summary>
public sealed class HttpResponseContext
{
    internal HttpResponseContext(HttpRequestMessage httpRequestMessage, IReadOnlyCollection<HttpRequestMessage> httpRequestMessages, HttpResponseMessage httpResponseMessage, TestableHttpMessageHandlerOptions? options = null)
    {
        HttpRequestMessage = httpRequestMessage;
        HttpRequestMessages = httpRequestMessages;
        HttpResponseMessage = httpResponseMessage;
        Options = options ?? new TestableHttpMessageHandlerOptions();
    }

    /// <summary>
    /// The request message that is sent by the HttpClient.
    /// </summary>
    public HttpRequestMessage HttpRequestMessage { get; }
    /// <summary>
    /// The requests that were send by the HttpClient.
    /// </summary>
    public IReadOnlyCollection<HttpRequestMessage> HttpRequestMessages { get; }
    /// <summary>
    /// The response message that will be sent back to the HttpClient.
    /// </summary>
    public HttpResponseMessage HttpResponseMessage { get; }
    /// <summary>
    /// The options that can be used by different responses.
    /// </summary>
    public TestableHttpMessageHandlerOptions Options { get; }
}
