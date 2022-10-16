using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class TimeoutResponseTests
{
    [Fact]
    public void GetResponseAsync_ByDefault_CancelsTheTaskAndToken()
    {
        TimeoutResponse sut = new();

        using CancellationTokenSource cancellationTokenSource = new();
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);
        var task = sut.ExecuteAsync(context, cancellationTokenSource.Token);

        Assert.True(cancellationTokenSource.IsCancellationRequested);
        Assert.True(task.IsCanceled);
    }

    [Fact]
    public async Task GetResponseAsync_WithCancelllationTokenWithoutSource_CancelsTheTaskAsync()
    {
        TimeoutResponse sut = new();
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        HttpResponseContext context = new(requestMessage, responseMessage);

        _ = await Assert.ThrowsAsync<TaskCanceledException>(() => sut.ExecuteAsync(context, CancellationToken.None));
    }
}
