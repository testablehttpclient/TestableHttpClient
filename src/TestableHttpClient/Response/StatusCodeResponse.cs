namespace TestableHttpClient.Response;

internal class StatusCodeResponse : ResponseBase
{
    public StatusCodeResponse(HttpStatusCode statusCode) => StatusCode = statusCode;
}
