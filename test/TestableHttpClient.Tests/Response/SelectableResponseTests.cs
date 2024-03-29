﻿using TestableHttpClient.Response;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.Tests.Response;

public class SelectableResponseTests
{
    [Fact]
    public void Constructor_NullSelector_ThrowsArgumentNullException()
    {
        Func<HttpResponseContext, IResponse> selectorFunc = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => new SelectableResponse(selectorFunc));
        Assert.Equal("selector", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_CallsSelector()
    {
        bool wasCalled = false;

        IResponse SelectResponse(HttpResponseContext _)
        {
            wasCalled = true;
            return StatusCode(HttpStatusCode.NoContent);
        }

        SelectableResponse sut = new(SelectResponse);

        using HttpResponseMessage responseMessage = await sut.TestAsync();

        Assert.True(wasCalled);
        Assert.Equal(HttpStatusCode.NoContent, responseMessage.StatusCode);
    }
}
