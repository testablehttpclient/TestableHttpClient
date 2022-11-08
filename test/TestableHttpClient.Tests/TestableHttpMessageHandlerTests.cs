using System.Collections.Concurrent;
using System.Threading;

using Moq;

namespace TestableHttpClient.Tests;

public class TestableHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_WhenRequestsAreMade_LogsRequests()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com/");

        _ = await client.SendAsync(request);

        Assert.Contains(request, sut.Requests);
    }

    [Fact]
    public async Task SendAsync_WhenMultipleRequestsAreMade_AllRequestsAreLogged()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);
        using var request1 = new HttpRequestMessage(HttpMethod.Get, "https://example1.com/");
        using var request2 = new HttpRequestMessage(HttpMethod.Post, "https://example2.com/");
        using var request3 = new HttpRequestMessage(HttpMethod.Delete, "https://example3.com/");
        using var request4 = new HttpRequestMessage(HttpMethod.Head, "https://example4.com/");

        _ = await client.SendAsync(request1);
        _ = await client.SendAsync(request2);
        _ = await client.SendAsync(request3);
        _ = await client.SendAsync(request4);

        Assert.Equal(new[] { request1, request2, request3, request4 }, sut.Requests);
    }

    [Fact]
    public async Task SendAsync_ByDefault_CallsExecutAsyncOnIResponse()
    {
        IResponse mockedResponse = Mock.Of<IResponse>();
        HttpResponseContext? context = null;
        Mock.Get(mockedResponse)
            .Setup(x => x.ExecuteAsync(It.IsAny<HttpResponseContext>(), It.IsAny<CancellationToken>()))
            .Callback((HttpResponseContext c, CancellationToken _) => context = c)
            .Returns(Task.CompletedTask);

        using TestableHttpMessageHandler sut = new();
        sut.RespondWith(mockedResponse);
        using var client = new HttpClient(sut);
        using HttpRequestMessage request = new(HttpMethod.Get, new Uri("https://example.com/"));
        var response = await client.SendAsync(request);

        Assert.NotNull(context);
        Assert.Same(request, context.HttpRequestMessage);
        Assert.Same(response, context.HttpResponseMessage);
        Assert.Same(sut.Options, context.Options);
    }

    [Fact]
    public async Task SendAsync_ByDefault_ReturnsHttpStatusCodeOK()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        var result = await client.GetAsync(new Uri("https://example.com/"));

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
        Assert.NotNull(result.RequestMessage);
    }

    [Fact]
    public async Task SendAsync_ByDefault_ReturnsDifferentResponseForEveryRequest()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        var result1 = await client.GetAsync(new Uri("https://example.com/"));
        var result2 = await client.GetAsync(new Uri("https://example.com/"));

        Assert.NotSame(result1, result2);
    }

    [Fact]
    public async Task SendAsync_ByDefault_SetsRequestMessageOnEveryResponse()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        using var request1 = new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/1"));
        using var request2 = new HttpRequestMessage(HttpMethod.Post, new Uri("https://example.com/2"));

        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        Assert.Same(request1, response1.RequestMessage);
        Assert.Same(request2, response2.RequestMessage);
        Assert.NotSame(response1.RequestMessage, response2.RequestMessage);
    }

    [Fact]
    public void RespondWith_NullResponse_ThrowArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();
        IResponse response = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(response));
        Assert.Equal("response", exception.ParamName);
    }

    [Fact]
    public async Task RespondWith_GivenResponse_ReturnsResponse()
    {
        using TestableHttpMessageHandler sut = new();
        sut.RespondWith(Responses.NoContent());

        using var client = new HttpClient(sut);
        var response = await client.GetAsync("https://example.com");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(response.RequestMessage);
    }

    [Fact]
    public void GetAsync_ShouldNotHang()
    {
        using var sut = new TestableHttpMessageHandler();
        sut.RespondWith(Responses.Delayed(new CustomResponse(), TimeSpan.FromSeconds(1)));

        var doesNotHang = Task.Run(() =>
        {
            SingleThreadedSynchronizationContext.Run(() =>
            {
                sut.CreateClient().GetAsync("http://example.com").GetAwaiter().GetResult();
            });
        }).Wait(TimeSpan.FromSeconds(10));


        Assert.True(doesNotHang);
    }

    private class CustomResponse : IResponse
    {
        public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
        {
#pragma warning disable CA1849 // Call async methods when in an async method
            Task.Delay(300, CancellationToken.None).GetAwaiter().GetResult();
            Responses.NoContent().ExecuteAsync(context, CancellationToken.None).GetAwaiter().GetResult();
#pragma warning restore CA1849 // Call async methods when in an async method
            return Task.CompletedTask;
        }
    }

    private class SingleThreadedSynchronizationContext : SynchronizationContext, IDisposable
    {
        private readonly BlockingCollection<(SendOrPostCallback Callback, object? State)> _queue = new();

        private SingleThreadedSynchronizationContext() { }
        public override void Send(SendOrPostCallback d, object? state) // Sync operations
        {
            throw new NotSupportedException($"{nameof(SingleThreadedSynchronizationContext)} does not support synchronous operations.");
        }

        public override void Post(SendOrPostCallback d, object? state) // Async operations
        {
            _queue.Add((d, state));
        }

        public static void Run(Action action)
        {
            var previous = Current;
            var context = new SingleThreadedSynchronizationContext();
            SetSynchronizationContext(context);
            try
            {
                action();

                while (context._queue.TryTake(out var item))
                {
                    item.Callback(item.State);
                }
            }
            finally
            {
                context._queue.CompleteAdding();
                SetSynchronizationContext(previous);
                context.Dispose();
            }
        }

        public void Dispose()
        {
            _queue.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
