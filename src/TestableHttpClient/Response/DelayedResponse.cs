namespace TestableHttpClient.Response;

internal class DelayedResponse : IResponse
{
    private readonly IResponse delayedResponse;
    private readonly int delayInMilliseconds;

    public DelayedResponse(IResponse delayedResponse, int delayInMilliseconds)
    {
        this.delayedResponse = delayedResponse;
        this.delayInMilliseconds = delayInMilliseconds;
    }

    public async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage)
    {
        await Task.Delay(delayInMilliseconds).ConfigureAwait(false);
        return await delayedResponse.GetResponseAsync(requestMessage).ConfigureAwait(false);
    }
}
