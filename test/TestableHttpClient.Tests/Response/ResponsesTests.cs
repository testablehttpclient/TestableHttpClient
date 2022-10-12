using System.Data.Common;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading;

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
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.IsType<TimeoutHttpResponseMessage>(response);
    }

    [Fact]
    public async Task StatusCode_ReturnsHttpResponseMessageWithStatusCode()
    {
        var sut = StatusCode(HttpStatusCode.Ambiguous);

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Equal(HttpStatusCode.Ambiguous, response.StatusCode);
    }

    [Fact]
    public async Task NoContent_ReturnsHttpResponseMessageWithNotContentStatusCode()
    {
        var sut = NoContent();

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delayed_ReturnsHttpResponseMessageWithADelay()
    {
        var sut = Delayed(NoContent(), 500);

        using HttpRequestMessage requestMessage = new();
        var stopwatch = Stopwatch.StartNew();
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        stopwatch.Stop();
        Assert.True(stopwatch.ElapsedMilliseconds >= 250);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Configured_ReturnsConfiguredHttpResponse()
    {
        var sut = Configured(NoContent(), x => x.Headers.Add("Server", "Mock"));

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal("Mock", response.Headers.Server.ToString());
    }

    [Fact]
    public async Task Sequence_ReturnsDifferentRequestWithEachCall()
    {
        var sut = Sequenced(NoContent(), StatusCode(HttpStatusCode.OK), StatusCode(HttpStatusCode.NotFound));

        using HttpRequestMessage requestMessage = new();
        var response1 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var response2 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var response3 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);
        var response4 = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Equal(HttpStatusCode.NoContent, response1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response3.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response4.StatusCode);
    }

    [Fact]
    public async Task Json_ReturnsCorrectJsonType()
    {
        var sut = Json("Charlie");

        using HttpRequestMessage requestMessage = new();
        var response = await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("\"Charlie\"", await response.Content!.ReadAsStringAsync());
    }
}
