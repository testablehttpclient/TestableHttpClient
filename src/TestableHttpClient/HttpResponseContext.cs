namespace TestableHttpClient;

public class HttpResponseContext
{
    public HttpResponseContext(HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage)
    {
        HttpRequestMessage = httpRequestMessage;
        HttpResponseMessage = httpResponseMessage;
    }

    public HttpRequestMessage HttpRequestMessage { get; }
    // The internal set is specifically for backwards compatibiltity of the HttpRepsonseMessage factory functionality.
    public HttpResponseMessage HttpResponseMessage { get; internal set; }
}
