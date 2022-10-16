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

    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        var response = GetResponse();
        return response.ExecuteAsync(context, cancellationToken);
    }

    private IResponse GetResponse()
    {
        if (responses.Any())
        {
            return responses.Dequeue();
        }
        else
        {
            return _lastResponse;
        }
    }
}
