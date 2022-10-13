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

    [Fact]
    public async Task GetResponseAsync_LargerDelay_ReturnsResponseWithDelay()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        long delayResponseCallTimestamp = 0;
        FunctionResponse delayedResponse = new(_ =>
        {
            delayResponseCallTimestamp = Stopwatch.GetTimestamp();
            return responseMessage;
        });
        DelayedResponse sut = new(delayedResponse, 500);
        var startTimeStamp = Stopwatch.GetTimestamp();
        await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.True(delayResponseCallTimestamp > 0, "No delay found");

        Assert.True(TimeSpan.FromTicks(delayResponseCallTimestamp - startTimeStamp).TotalMilliseconds > 10);
    }

    [Fact]
    public async Task GetResponseAsync_0Delay_ReturnsResponseWithoutDelay()
    {
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        long delayResponseCallTimestamp = 0;
        FunctionResponse delayedResponse = new(_ =>
        {
            delayResponseCallTimestamp = Stopwatch.GetTimestamp();
            return responseMessage;
        });
        DelayedResponse sut = new(delayedResponse, 0);
        var startTimeStamp = Stopwatch.GetTimestamp();
        await sut.GetResponseAsync(requestMessage, CancellationToken.None);

        Assert.True(delayResponseCallTimestamp > 0, "No delay found");
        Assert.True(TimeSpan.FromTicks(delayResponseCallTimestamp - startTimeStamp).TotalMilliseconds < 10);
    }
}
