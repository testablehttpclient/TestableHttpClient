namespace TestableHttpClient.Response;

internal class TimeoutResponse : ResponseBase
{
    protected override HttpResponseMessage GetResponse(HttpRequestMessage requestMessage)
        => new TimeoutHttpResponseMessage();
}
