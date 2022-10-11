namespace TestableHttpClient.Response;

internal class JsonResponse : ResponseBase
{
    private readonly object content;

    public JsonResponse(object content)
    {
        this.content = content;
    }

    protected override HttpResponseMessage GetResponse(HttpRequestMessage requestMessage)
    {
        HttpResponseMessage response = new();
        string json = JsonSerializer.Serialize(content);
        response.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return response;
    }
}
