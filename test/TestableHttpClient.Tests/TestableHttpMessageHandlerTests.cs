using System.Collections.Concurrent;
using System.Threading;

using NSubstitute;

namespace TestableHttpClient.Tests;

public class TestableHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_WhenRequestsAreMade_LogsRequests()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com/");

        _ = await client.SendAsync(request);

        Assert.Contains(request, sut.Requests);
    }

    [Fact]
    public async Task SendAsync_WhenMultipleRequestsAreMade_AllRequestsAreLogged()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);
        using HttpRequestMessage request1 = new(HttpMethod.Get, "https://example1.com/");
        using HttpRequestMessage request2 = new(HttpMethod.Post, "https://example2.com/");
        using HttpRequestMessage request3 = new(HttpMethod.Delete, "https://example3.com/");
        using HttpRequestMessage request4 = new(HttpMethod.Head, "https://example4.com/");

        _ = await client.SendAsync(request1);
        _ = await client.SendAsync(request2);
        _ = await client.SendAsync(request3);
        _ = await client.SendAsync(request4);

        Assert.Equal([ request1, request2, request3, request4 ], sut.Requests);
    }

    [Fact]
    public async Task SendAsync_ByDefault_CallsExecutAsyncOnIResponse()
    {
        IResponse mockedResponse = Substitute.For<IResponse>();
        HttpResponseContext? context = null;
        mockedResponse.ExecuteAsync(Arg.Any<HttpResponseContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask)
            .AndDoes(x => context = x[0] as HttpResponseContext);

        using TestableHttpMessageHandler sut = new();
        sut.RespondWith(mockedResponse);
        using HttpClient client = new(sut);
        using HttpRequestMessage request = new(HttpMethod.Get, new Uri("https://example.com/"));
        using HttpResponseMessage response = await client.SendAsync(request);

        Assert.NotNull(context);
        Assert.Same(request, context.HttpRequestMessage);
        Assert.Same(response, context.HttpResponseMessage);
        Assert.Same(sut.Options, context.Options);
    }

    [Fact]
    public async Task SendAsync_ByDefault_ReturnsHttpStatusCodeOK()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);

        using HttpResponseMessage result = await client.GetAsync(new Uri("https://example.com/"));

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
        Assert.NotNull(result.RequestMessage);
    }

    [Fact]
    public async Task SendAsync_ByDefault_ReturnsDifferentResponseForEveryRequest()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);

        using HttpResponseMessage result1 = await client.GetAsync(new Uri("https://example.com/"));
        using HttpResponseMessage result2 = await client.GetAsync(new Uri("https://example.com/"));

        Assert.NotSame(result1, result2);
    }

    [Fact]
    public async Task SendAsync_ByDefault_SetsRequestMessageOnEveryResponse()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);

        using HttpRequestMessage request1 = new(HttpMethod.Get, new Uri("https://example.com/1"));
        using HttpRequestMessage request2 = new(HttpMethod.Post, new Uri("https://example.com/2"));

        using HttpResponseMessage response1 = await client.SendAsync(request1);
        using HttpResponseMessage response2 = await client.SendAsync(request2);

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
        sut.RespondWith(Responses.StatusCode(HttpStatusCode.NoContent));

        using HttpClient client = new(sut);
        using HttpResponseMessage response = await client.GetAsync("https://example.com");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(response.RequestMessage);
    }

    [Fact]
    public async Task ClearRequests_ByDefault_ShouldClearRequests()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);
        _ = await client.GetAsync("https://example.com");

        Assert.NotEmpty(sut.Requests);

        sut.ClearRequests();

        Assert.Empty(sut.Requests);
    }

    [Fact]
    [SuppressMessage("Usage", "xUnit1031:Do not use blocking task operations in test method", Justification = "Here it is necessary, if it blocks the test, it will time out and breaks the test.")]
    public void GetAsync_ShouldNotHang()
    {
        using TestableHttpMessageHandler sut = new();
        sut.RespondWith(Responses.Delayed(new CustomResponse(), TimeSpan.FromSeconds(1)));

        bool doesNotHang = Task.Run(() =>
        {
            SingleThreadedSynchronizationContext.Run(() =>
            {
                sut.CreateClient().GetAsync("http://example.com").GetAwaiter().GetResult();
            });
        }).Wait(TimeSpan.FromSeconds(10));


        Assert.True(doesNotHang);
    }

    private sealed class CustomResponse : IResponse
    {
        public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken)
        {
#pragma warning disable CA1849 // Call async methods when in an async method
            Task.Delay(300, CancellationToken.None).GetAwaiter().GetResult();
            Responses.StatusCode(HttpStatusCode.NoContent).ExecuteAsync(context, CancellationToken.None).GetAwaiter().GetResult();
#pragma warning restore CA1849 // Call async methods when in an async method
            return Task.CompletedTask;
        }
    }

    private sealed class SingleThreadedSynchronizationContext : SynchronizationContext, IDisposable
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
            SynchronizationContext? previous = Current;
            using var context = new SingleThreadedSynchronizationContext();
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
            }
        }

        public void Dispose()
        {
            _queue.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
