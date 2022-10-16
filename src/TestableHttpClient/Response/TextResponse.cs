namespace TestableHttpClient.Response;

internal class TextResponse : HttpResponse
{
    public TextResponse(string content, Encoding? encoding = null, string? mediaType = null)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Encoding = encoding ?? Encoding.UTF8;
        MediaType = mediaType ?? "text/plain";
    }

    public string Content { get; }
    public Encoding Encoding { get; }
    public string MediaType { get; }

    protected override Task<HttpContent?> GetContentAsync(HttpResponseContext context)
    {
        return Task.FromResult<HttpContent?>(new StringContent(Content, Encoding, MediaType));
    }
}
