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
    public async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
    {
        var response = await innerResponse.GetResponseAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        configureResponse(response);
        return response;
    }
}
