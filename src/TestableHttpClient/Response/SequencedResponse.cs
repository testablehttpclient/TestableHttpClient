namespace TestableHttpClient.Response;

internal class SequencedResponse : IResponse
{
    private readonly Queue<IResponse> responses;
    private readonly IResponse _lastResponse;
    public SequencedResponse(IEnumerable<IResponse> responses)
    {
        this.responses = new(responses ?? throw new ArgumentNullException(nameof(responses)));
        if (this.responses.Count == 0)
        {
            throw new ArgumentException("Responses can't be empty.", nameof(responses));
        }
        _lastResponse = this.responses.Last();
    }

    public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
    {
        var response = GetResponse();
        return response.GetResponseAsync(requestMessage, cancellationToken);
    }

    private IResponse GetResponse()
    {
        if (responses.TryDequeue(out var response))
        {
            return response;
        }
        else
        {
            return _lastResponse;
        }
    }
}
