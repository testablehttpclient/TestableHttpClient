﻿using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class FunctionResponseTests
{
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
        using HttpResponseMessage responseMessage = new();

        var builderWasCalled = false;

        Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction = builder =>
        {
            builderWasCalled = true;
            builder.WithHttpStatusCode(HttpStatusCode.Ambiguous);
        };

        FunctionResponse response = new(httpResponseMessageBuilderAction);

        await response.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.True(builderWasCalled);
        Assert.Equal(HttpStatusCode.Ambiguous, responseMessage.StatusCode);
    }
}
