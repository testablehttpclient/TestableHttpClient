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

    public async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
    {
        await Task.Delay(delayInMilliseconds, cancellationToken);
        return await delayedResponse.GetResponseAsync(requestMessage, cancellationToken);
    }
}
