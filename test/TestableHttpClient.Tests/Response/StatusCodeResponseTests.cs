using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class StatusCodeResponseTests
{
    [Fact]
    public async Task GetReponseAsync_WithHttpStatusCode_ReturnsCorrectStatusCode()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        StatusCodeResponse sut = new(HttpStatusCode.Continue);

        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.Equal(HttpStatusCode.Continue, responseMessage.StatusCode);
        Assert.Equal(HttpStatusCode.Continue, sut.StatusCode);
    }
}
