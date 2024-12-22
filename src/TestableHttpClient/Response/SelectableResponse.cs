namespace TestableHttpClient.Response;

internal sealed class SelectableResponse : IResponse
{
    private readonly Func<HttpResponseContext, IResponse> selector;

    public SelectableResponse(Func<HttpResponseContext, IResponse> selector)
    {
        this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
    }

    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        var response = selector(context);
        return response.ExecuteAsync(context, cancellationToken);
    }
}
