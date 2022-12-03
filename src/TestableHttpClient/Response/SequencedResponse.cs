namespace TestableHttpClient.Response;

internal class SequencedResponse : IResponse
{
    private readonly List<IResponse> responses;
    public SequencedResponse(IEnumerable<IResponse> responses)
    {
        this.responses = new(responses ?? throw new ArgumentNullException(nameof(responses)));
        if (this.responses.Count == 0)
        {
            throw new ArgumentException("Responses can't be empty.", nameof(responses));
        }
    }

    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
    {
        int responseIndex = Math.Min(responses.Count - 1, context.HttpRequestMessages.Count - 1);

        IResponse response = responses[responseIndex];
        return response.ExecuteAsync(context, cancellationToken);
    }
}
