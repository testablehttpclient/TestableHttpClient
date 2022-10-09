using System.Data.Common;
using System.Diagnostics;

using TestableHttpClient.Utils;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.Tests.Response;

public class ResponsesTests
{
    [Fact]
    public async Task Timeout_ReturnsTimeoutHttpResponseMessage()
    {
        var sut = Timeout();

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage);

        Assert.IsType<TimeoutHttpResponseMessage>(response);
    }

    [Fact]
    public async Task StatusCode_ReturnsHttpResponseMessageWithStatusCode()
    {
        var sut = StatusCode(HttpStatusCode.Ambiguous);

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage);

        Assert.Equal(HttpStatusCode.Ambiguous, response.StatusCode);
    }

    [Fact]
    public async Task NoContent_ReturnsHttpResponseMessageWithNotContentStatusCode()
    {
        var sut = NoContent();

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delayed_ReturnsHttpResponseMessageWithADelay()
    {
        var sut = Delayed(NoContent(), 500);

        using HttpRequestMessage requestMessage = new();
        var stopwatch = Stopwatch.StartNew();
        var response = await sut.GetResponseAsync(requestMessage);
        stopwatch.Stop();
        Assert.InRange(stopwatch.ElapsedMilliseconds, 450, 550);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Configured_ReturnsConfiguredHttpResponse()
    {
        var sut = Configured(NoContent(), x => x.Headers.Add("Server", "Mock"));

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("Mock", response.Headers.Server.ToString());
    }
}
