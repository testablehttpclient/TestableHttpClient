namespace TestableHttpClient.Response;

internal class JsonResponse : HttpResponse
{
    public JsonResponse(object? content, string? contentType = null)
    {
        Content = content;
        ContentType = contentType ?? "application/json";
    }

    public object? Content { get; }
    public string ContentType { get; }

    protected override Task<HttpContent?> GetContentAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        string json = JsonSerializer.Serialize(Content);
        return Task.FromResult<HttpContent?>(new StringContent(json, Encoding.UTF8, ContentType));
    }
}
