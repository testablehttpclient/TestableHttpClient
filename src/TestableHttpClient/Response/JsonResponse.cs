namespace TestableHttpClient.Response;

internal sealed class JsonResponse : HttpResponse
{
    public JsonResponse(object? content, string? contentType = null)
    {
        Content = content;
        ContentType = contentType ?? "application/json";
    }

    public object? Content { get; }
    public string ContentType { get; }
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }

    protected override Task<HttpContent?> GetContentAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        string json = JsonSerializer.Serialize(Content, JsonSerializerOptions ?? context.Options.JsonSerializerOptions);
        return Task.FromResult<HttpContent?>(new StringContent(json, Encoding.UTF8, ContentType));
    }
}
