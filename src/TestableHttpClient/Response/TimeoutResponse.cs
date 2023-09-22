namespace TestableHttpClient.Response;

internal class TimeoutResponse : IResponse
{
    public async Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        var cancelationSource = cancellationToken.GetSource();

        if (cancelationSource is not null)
        {
#if NET8_0_OR_GREATER
            await cancelationSource.CancelAsync().ConfigureAwait(true);
#else
            cancelationSource.Cancel(false);
            await Task.FromCanceled<HttpResponseMessage>(cancellationToken).ConfigureAwait(true);
#endif
        }

        throw new TaskCanceledException();
    }
}
