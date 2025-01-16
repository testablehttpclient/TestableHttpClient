namespace TestableHttpClient.Response;

internal sealed class DelayedResponse : IResponse
{
    private readonly IResponse delayedResponse;
    private readonly TimeSpan delay;

    public DelayedResponse(IResponse delayedResponse, TimeSpan delay)
    {
        this.delayedResponse = delayedResponse ?? throw new ArgumentNullException(nameof(delayedResponse));
        this.delay = delay;
    }

    public async Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
        await delayedResponse.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);
    }
}
