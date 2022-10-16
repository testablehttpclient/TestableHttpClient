namespace TestableHttpClient.Response;

internal class TimeoutResponse : IResponse
{
    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        var cancelationSource = cancellationToken.GetSource();

        if (cancelationSource is not null)
        {
            cancelationSource.Cancel(false);
            return Task.FromCanceled<HttpResponseMessage>(cancellationToken);
        }

        throw new TaskCanceledException();
    }
}
