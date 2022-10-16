using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class HttpResponseTests
{
    [Fact]
    public async Task ExecuteAsync_WithNullContext_ThrowsArgumentNullException()
    {
        HttpResponse sut = new(HttpStatusCode.Continue);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!, CancellationToken.None));

        Assert.Equal("context", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WithHttpStatusCode_ReturnsCorrectStatusCode()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        HttpResponse sut = new(HttpStatusCode.Continue);

        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Continue, responseMessage.StatusCode);
        Assert.Equal(HttpStatusCode.Continue, sut.StatusCode);
    }
}
