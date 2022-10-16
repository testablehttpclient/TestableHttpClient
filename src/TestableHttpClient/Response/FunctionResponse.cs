namespace TestableHttpClient.Response;

internal class FunctionResponse : IResponse
{
    private readonly Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction;

    internal FunctionResponse(Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction)
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
