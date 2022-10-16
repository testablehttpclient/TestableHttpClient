namespace TestableHttpClient.Response;

internal class TextResponse : ResponseBase
{
    public TextResponse(string content)
    {
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }

    public string Content { get; }

    protected override HttpContent? GetContent(HttpResponseContext context)
    {
        return new StringContent(Content, Encoding.UTF8, "text/plain");
    }
}
