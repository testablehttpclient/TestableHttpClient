namespace TestableHttpClient.Response;

internal class TimeoutResponse : IResponse
{
    public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
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
