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
    public async Task GetResponseAsync_WithSingleResponse_ReturnsSameResponseEveryCall()
    {
        HttpResponse innerResponse = new(HttpStatusCode.Created);
        SequencedResponse sut = new([ innerResponse ]);
        using TestableHttpMessageHandler handler = new();
        handler.RespondWith(sut);

        using HttpResponseMessage responseMessage1 = await handler.TestAsync();
        using HttpResponseMessage responseMessage2 = await handler.TestAsync();
        using HttpResponseMessage responseMessage3 = await handler.TestAsync();

        Assert.Equal(HttpStatusCode.Created, responseMessage1.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseMessage2.StatusCode);
        Assert.Equal(HttpStatusCode.Created, responseMessage3.StatusCode);
    }

    [Fact]
    public async Task GetResponseAsync_WithMultipleResponses_ReturnsDifferentResponseForEachRequest()
    {
        SequencedResponse sut = new([
            new HttpResponse(HttpStatusCode.Created),
            new HttpResponse(HttpStatusCode.Accepted),
            new HttpResponse(HttpStatusCode.NoContent),
        ]);

        using TestableHttpMessageHandler handler = new();
        handler.RespondWith(sut);

        using HttpResponseMessage responseMessage1 = await handler.TestAsync();
        using HttpResponseMessage responseMessage2 = await handler.TestAsync();
        using HttpResponseMessage responseMessage3 = await handler.TestAsync();
        using HttpResponseMessage responseMessage4 = await handler.TestAsync();

        Assert.Equal(HttpStatusCode.Created, responseMessage1.StatusCode);
        Assert.Equal(HttpStatusCode.Accepted, responseMessage2.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, responseMessage3.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, responseMessage4.StatusCode);
    }

    [Fact]
    public async Task GetResponseAsync_AfterHandlerReset_ReturnsCorrectResponseForEachRequest()
    {
        SequencedResponse sut = new([
            new HttpResponse(HttpStatusCode.Created),
            new HttpResponse(HttpStatusCode.Accepted),
            new HttpResponse(HttpStatusCode.NoContent),
        ]);

        using TestableHttpMessageHandler handler = new();
        handler.RespondWith(sut);

        _ = await handler.TestAsync();
        _ = await handler.TestAsync();

        handler.ClearRequests();

        using HttpResponseMessage responseMessage1 = await handler.TestAsync();
        using HttpResponseMessage responseMessage2 = await handler.TestAsync();
        using HttpResponseMessage responseMessage3 = await handler.TestAsync();
        using HttpResponseMessage responseMessage4 = await handler.TestAsync();

        Assert.Equal(HttpStatusCode.Created, responseMessage1.StatusCode);
        Assert.Equal(HttpStatusCode.Accepted, responseMessage2.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, responseMessage3.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, responseMessage4.StatusCode);
    }
}
