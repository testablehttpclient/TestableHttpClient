namespace TestableHttpClient.Response;

internal class DelayedResponse : IResponse
{
    private readonly IResponse delayedResponse;
    private readonly int delayInMilliseconds;

    public DelayedResponse(IResponse delayedResponse, int delayInMilliseconds)
    {
        this.delayedResponse = delayedResponse ?? throw new ArgumentNullException(nameof(delayedResponse));
        this.delayInMilliseconds = delayInMilliseconds;
    }

    public async Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(delayInMilliseconds, cancellationToken).ConfigureAwait(false);
        await delayedResponse.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);
    }
}
