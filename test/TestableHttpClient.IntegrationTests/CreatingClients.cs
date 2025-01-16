using System.Threading;

namespace TestableHttpClient.IntegrationTests;

public class CreatingClients
{
    [Fact]
    public async Task CreateASimpleHttpClient()
    {
        using TestableHttpMessageHandler testableHttpMessageHandler = new();
        using HttpClient client = testableHttpMessageHandler.CreateClient();

        await client.GetAsync("https://httpbin.org/get", TestContext.Current.CancellationToken);

        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/get");
    }

    [Fact]
    public async Task CreateClientWithConfiguration()
    {
        using TestableHttpMessageHandler testableHttpMessageHandler = new();
        using HttpClient client = testableHttpMessageHandler.CreateClient(client => client.DefaultRequestHeaders.Add("test", "test"));

        await client.GetAsync("https://httpbin.org/get", TestContext.Current.CancellationToken);

        testableHttpMessageHandler.ShouldHaveMadeRequests().WithRequestHeader("test", "test");
    }

    [Fact]
    public async Task CreateClientWithCustomHandlers()
    {
        using TestableHttpMessageHandler testableHttpMessageHandler = new();
        using TestHandler handler = new();
        using HttpClient client = testableHttpMessageHandler.CreateClient(handler);

        await client.GetAsync("https://httpbin.org/get", TestContext.Current.CancellationToken);

        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/get");
        Assert.True(handler.WasCalled);
    }

    private sealed class TestHandler : DelegatingHandler
    {
        public TestHandler() { }
        public bool WasCalled { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            WasCalled = true;
            return base.SendAsync(request, cancellationToken);
        }
    }
}
