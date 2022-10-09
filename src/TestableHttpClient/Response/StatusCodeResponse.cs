namespace TestableHttpClient.Response;

internal class StatusCodeResponse : ResponseBase
{
    public StatusCodeResponse(HttpStatusCode statusCode) => StatusCode = statusCode;

    public HttpStatusCode StatusCode { get; }

    protected override HttpResponseMessage GetResponse(HttpRequestMessage requestMessage)
        => new HttpResponseMessage(StatusCode);
}
