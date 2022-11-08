using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

[Obsolete("Use ConfiguredResponse or a custom IResponse instead.")]
public class BuilderResponseTests
{
    [Fact]
    public void Constructor_WithNullHttpResponseMessageBuilder_ThrowsArgumentNullException()
    {
        Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => new BuilderResponse(httpResponseMessageBuilderAction));
        Assert.Equal("httpResponseMessageBuilderAction", exception.ParamName);
    }

    [Fact]
    public async Task GetResponseAsync_WithHttpResponseMessageBuilder_CallsBuilderWhenSendingResponse()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();

        var builderWasCalled = false;

        void httpResponseMessageBuilderAction(HttpResponseMessageBuilder builder)
        {
            builderWasCalled = true;
            builder.WithHttpStatusCode(HttpStatusCode.Ambiguous);
        }

        BuilderResponse response = new(httpResponseMessageBuilderAction);

        await response.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.True(builderWasCalled);
        Assert.Equal(HttpStatusCode.Ambiguous, responseMessage.StatusCode);
    }
}
