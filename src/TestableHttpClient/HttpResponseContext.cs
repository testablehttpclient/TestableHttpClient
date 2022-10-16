namespace TestableHttpClient;

/// <summary>
/// This class contains contextual information for generating responses.
/// </summary>
public class HttpResponseContext
{
    public HttpResponseContext(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage)
    {
        HttpRequestMessage = httpRequestMessage;
        HttpResponseMessage = httpResponseMessage;
    }

    /// <summary>
    /// The request message that is send by the HttpClient.
    /// </summary>
    public HttpRequestMessage HttpRequestMessage { get; }
    /// <summary>
    /// The response message that will be send back to the HttpClient.
    /// </summary>
    public HttpResponseMessage HttpResponseMessage { get; }
}
