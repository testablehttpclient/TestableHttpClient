using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class FunctionResponseTests
{
    [Fact]
    public void Constructor_WithNullHttpResponseMessageFactory_ThrowsArgumentNullException()
    {
        Func<HttpRequestMessage, HttpResponseMessage> httpResponseMessageFactory = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => new FunctionResponse(httpResponseMessageFactory));
        Assert.Equal("httpResponseMessageFactory", exception.ParamName);
    }

    [Fact]
    public async Task GetResponseAsync_WithHttpResponseMessageFactory_ReturnsHttpResponseMessageFromFactory()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();

        Func<HttpRequestMessage, HttpResponseMessage> httpResponseMessageFactory = request =>
        {
            Assert.Same(requestMessage, request);
            return responseMessage;
        };

        FunctionResponse response = new(httpResponseMessageFactory);

        var result = await response.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Same(responseMessage, result);
    }

    [Fact]
    public void Constructor_WithNullHttpResponseMessageBuilder_ThrowsArgumentNullException()
    {
        Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => new FunctionResponse(httpResponseMessageBuilderAction));
        Assert.Equal("httpResponseMessageBuilderAction", exception.ParamName);
    }

    [Fact]
    public async Task GetResponseAsync_WithHttpResponseMessageBuilder_CallsBuilderWhenSendingResponse()
    {
        using HttpRequestMessage requestMessage = new();
        var builderWasCalled = false;

        Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction = builder =>
        {
            builderWasCalled = true;
            builder.WithHttpStatusCode(HttpStatusCode.Ambiguous);
        };

        FunctionResponse response = new(httpResponseMessageBuilderAction);

        var result = await response.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.True(builderWasCalled);
        Assert.Equal(HttpStatusCode.Ambiguous, result.StatusCode);
    }
}
