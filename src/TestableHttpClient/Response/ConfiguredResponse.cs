namespace TestableHttpClient.Response;

internal class ConfiguredResponse : IResponse
{
    private readonly IResponse innerResponse;
    private readonly Action<HttpResponseMessage> configureResponse;

    public ConfiguredResponse(IResponse response, Action<HttpResponseMessage> configureResponse)
    {
        innerResponse = response ?? throw new ArgumentNullException(nameof(response));
        this.configureResponse = configureResponse ?? throw new ArgumentNullException(nameof(configureResponse));
    }

    public async Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        await innerResponse.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);
        configureResponse(context.HttpResponseMessage);
    }
}
