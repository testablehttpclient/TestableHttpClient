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
        HttpResponse sut = new(HttpStatusCode.Continue);

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.Equal(HttpStatusCode.Continue, responseMessage.StatusCode);
        Assert.Equal(HttpStatusCode.Continue, sut.StatusCode);
    }
}
