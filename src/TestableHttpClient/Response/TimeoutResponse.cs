namespace TestableHttpClient.Response;

internal sealed class TimeoutResponse : IResponse
{
    public async Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        var cancelationSource = cancellationToken.GetSource();

        if (cancelationSource is not null)
        {
#if NETSTANDARD
            cancelationSource.Cancel(false);
            await Task.FromCanceled<HttpResponseMessage>(cancellationToken).ConfigureAwait(true);
#else
            await cancelationSource.CancelAsync().ConfigureAwait(true);
#endif
        }

        throw new TaskCanceledException();
    }
}
