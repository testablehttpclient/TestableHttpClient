using System.Collections.Concurrent;

namespace TestableHttpClient.Response;

internal class SequencedResponse : IResponse
{
    private readonly ConcurrentQueue<IResponse> responses;
    private readonly IResponse _lastResponse;
    public SequencedResponse(IEnumerable<IResponse> responses)
    {
        this.responses = new(responses ?? throw new ArgumentNullException(nameof(responses)));
        if (this.responses.IsEmpty)
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
        return responses.TryDequeue(out var response) ? response : _lastResponse;
    }
}
