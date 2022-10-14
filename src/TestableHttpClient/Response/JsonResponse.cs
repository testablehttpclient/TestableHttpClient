namespace TestableHttpClient.Response;

internal class JsonResponse : ResponseBase
{
    public JsonResponse(object? content)
    {
        Content = content;
    }

    public object? Content { get; }

    protected override HttpContent? GetContent(HttpRequestMessage requestMessage)
    {
        string json = JsonSerializer.Serialize(Content);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
