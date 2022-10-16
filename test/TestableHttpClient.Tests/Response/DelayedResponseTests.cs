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

        var exception = Assert.Throws<ArgumentNullException>(() => new DelayedResponse(response, 1));

        Assert.Equal("delayedResponse", exception.ParamName);
    }

    // Note: The delay itself can't be tested reliably.

    [Fact]
    public async Task GetResponseAsync_ByDefault_ReturnsInnerResponse()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        FunctionResponse delayedResponse = new(_ => responseMessage);
        DelayedResponse sut = new(delayedResponse, 500);

        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Same(responseMessage, response);
    }
}
