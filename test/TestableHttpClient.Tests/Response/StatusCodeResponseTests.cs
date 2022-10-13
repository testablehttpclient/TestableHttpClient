﻿using System.Threading;

using TestableHttpClient.Response;
using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Response;

public class StatusCodeResponseTests
{
    [Fact]
    public async Task GetReponseAsync_WithHttpStatusCode_ReturnsCorrectStatusCode()
    {
        using HttpRequestMessage requestMessage = new();
        StatusCodeResponse sut = new(HttpStatusCode.Continue);

        var result = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Equal(HttpStatusCode.Continue, result.StatusCode);
    }
}