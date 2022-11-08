namespace TestableHttpClient.Response;

internal class HttpResponse : IResponse
{
    protected HttpResponse() { }
    public HttpResponse(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

    protected virtual Task<HttpContent?> GetContentAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        return Task.FromResult<HttpContent?>(null);
    }

    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        return ExecuteAsyncImpl(context, cancellationToken);
    }

    private async Task ExecuteAsyncImpl(HttpResponseContext context, CancellationToken cancellationToken)
    {
        context.HttpResponseMessage.StatusCode = StatusCode;
        var content = await GetContentAsync(context, cancellationToken).ConfigureAwait(false);
        if (content is not null)
        {
            context.HttpResponseMessage.Content = content;
        }
    }
}
