namespace TestableHttpClient.Response;

internal class ConfiguredResponse : IResponse
{
    private readonly IResponse innerResponse;
    private readonly Action<HttpResponseMessage> configureResponse;

    public ConfiguredResponse(IResponse response, Action<HttpResponseMessage> configureResponse)
    {
        innerResponse = response;
        this.configureResponse = configureResponse;
    }
    public async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage)
    {
        var response = await innerResponse.GetResponseAsync(requestMessage);
        configureResponse(response);
        return response;
    }
}
