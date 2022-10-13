using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class SequencedResponseTests
{
    [Fact]
    public void Constructor_NullCollection_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new SequencedResponse(null!));
        Assert.Equal("responses", exception.ParamName);
    }

    [Fact]
    public void Constructor_EmptyCollection_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => new SequencedResponse(Array.Empty<IResponse>()));
        Assert.Equal("responses", exception.ParamName);
        Assert.Contains("can't be empty", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetReponseAsync_WithSingleResponse_ReturnsSameResponseEveryCall()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        FunctionResponse innerResponse = new(_ => responseMessage);
        var sut = new SequencedResponse(new[] { innerResponse });

        var result1 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var result2 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var result3 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Same(responseMessage, result1);
        Assert.Same(responseMessage, result2);
        Assert.Same(responseMessage, result3);
    }

    [Fact]
    public async Task GetReponseAsync_WithMultpleResponses_ReturnsDifferentResponseForEachRequest()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage1 = new();
        using HttpResponseMessage responseMessage2 = new();
        using HttpResponseMessage responseMessage3 = new();
        
        var sut = new SequencedResponse(new[]
        {
            new FunctionResponse(_ => responseMessage1),
            new FunctionResponse(_ => responseMessage2),
            new FunctionResponse(_ => responseMessage3),
        });

        var result1 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var result2 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var result3 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var result4 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Same(responseMessage1, result1);
        Assert.Same(responseMessage2, result2);
        Assert.Same(responseMessage3, result3);
        Assert.Same(responseMessage3, result4);
    }
}
