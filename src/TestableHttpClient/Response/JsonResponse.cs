namespace TestableHttpClient.Response;

internal class JsonResponse : HttpResponse
{
    public JsonResponse(object? content, Encoding? encoding = null, string? mediaType = null)
    {
        Content = content;
        Encoding = encoding ?? Encoding.UTF8;
        MediaType = mediaType ?? "application/json";
    }

    public object? Content { get; }
    public Encoding Encoding { get; }
    public string MediaType { get; }

    protected override Task<HttpContent?> GetContentAsync(HttpResponseContext context)
    {
        string json = JsonSerializer.Serialize(Content);
        return Task.FromResult<HttpContent?>(new StringContent(json, Encoding, MediaType));
    }
}
