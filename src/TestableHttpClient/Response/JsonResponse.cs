namespace TestableHttpClient.Response;

internal class JsonResponse : HttpResponse
{
    public JsonResponse(object? content)
    {
        Content = content;
    }

    public object? Content { get; }

    protected override HttpContent? GetContent(HttpResponseContext context)
    {
        string json = JsonSerializer.Serialize(Content);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
