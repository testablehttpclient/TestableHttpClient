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
        var task = sut.GetResponseAsync(requestMessage, cancellationTokenSource.Token);

        Assert.True(cancellationTokenSource.IsCancellationRequested);
        Assert.True(task.IsCanceled);
    }

    [Fact]
    public async Task GetResponseAsync_WithCancelllationTokenWithoutSource_CancelsTheTaskAsync()
    {
        TimeoutResponse sut = new();

        _ = await Assert.ThrowsAsync<TaskCanceledException>(() => sut.GetResponseAsync(new HttpRequestMessage(), CancellationToken.None));
    }
}
