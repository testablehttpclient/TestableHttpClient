using System.Diagnostics;
using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class DelayedResponseTests
{
    [Fact]
    public void Constructor_NullResponse_ThrowsArgumentNullException()
    {
        IResponse response = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => new DelayedResponse(response, TimeSpan.Zero));

        Assert.Equal("delayedResponse", exception.ParamName);
    }

    // Note: The delay itself can't be tested reliably, so use a 0 delay.

    [Fact]
    public async Task GetResponseAsync_ByDefault_ReturnsInnerResponse()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        HttpResponse delayedResponse = new(HttpStatusCode.Created);
        DelayedResponse sut = new(delayedResponse, TimeSpan.Zero);

        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
    }
}
