namespace TestableHttpClient.Response;

internal class JsonResponse : IResponse
{
    private readonly object content;

    public JsonResponse(object content)
    {
        this.content = content;
    }

    public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage)
    {
        throw new NotImplementedException();
    }
}
