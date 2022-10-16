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
        StatusCodeResponse innerResponse = new(HttpStatusCode.Created);
        var sut = new SequencedResponse(new[] { innerResponse });

        using HttpResponseMessage responseMessage1 = new();
        using HttpResponseMessage responseMessage2 = new();
        using HttpResponseMessage responseMessage3 = new();

        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage1), CancellationToken.None);
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage2), CancellationToken.None);
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage3), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Created, responseMessage1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseMessage2.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseMessage3.StatusCode);
    }

    [Fact]
    public async Task GetReponseAsync_WithMultpleResponses_ReturnsDifferentResponseForEachRequest()
    {
        using HttpRequestMessage requestMessage = new();
        
        var sut = new SequencedResponse(new[]
        {
            new StatusCodeResponse(HttpStatusCode.Created),
            new StatusCodeResponse(HttpStatusCode.Accepted),
            new StatusCodeResponse(HttpStatusCode.NoContent),
        });

        using HttpResponseMessage responseMessage1 = new();
        using HttpResponseMessage responseMessage2 = new();
        using HttpResponseMessage responseMessage3 = new();
        using HttpResponseMessage responseMessage4 = new();

        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage1), CancellationToken.None);
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage2), CancellationToken.None);
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage3), CancellationToken.None);
        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage4), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Created, responseMessage1.StatusCode);
        Assert.Equal(HttpStatusCode.Accepted, responseMessage2.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, responseMessage3.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, responseMessage4.StatusCode);
    }
}
