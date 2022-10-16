namespace TestableHttpClient.Response;

[Obsolete("Use ConfiguredResponse or a custom IResponse instead.")]
internal class BuilderResponse : IResponse
{
    private readonly Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction;

    internal BuilderResponse(Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction)
    {
        this.httpResponseMessageBuilderAction = httpResponseMessageBuilderAction ?? throw new ArgumentNullException(nameof(httpResponseMessageBuilderAction));
    }

    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        HttpResponseMessageBuilder builder = new(context.HttpResponseMessage);
        httpResponseMessageBuilderAction(builder);

        return Task.CompletedTask;
    }
}
