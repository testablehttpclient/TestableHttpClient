using System.Threading;

namespace TestableHttpClient.IntegrationTests;

public class CreatingClients
{
    [Fact]
    public async Task CreateASimpleHttpClient()
    {
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        using var client = testableHttpMessageHandler.CreateClient();

        await client.GetAsync("https://httpbin.org/get");

        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/get");
    }

    [Fact]
    public async Task CreateClientWithConfiguration()
    {
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        using var client = testableHttpMessageHandler.CreateClient(client => client.DefaultRequestHeaders.Add("test", "test"));

        await client.GetAsync("https://httpbin.org/get");

        testableHttpMessageHandler.ShouldHaveMadeRequests().WithRequestHeader("test", "test");
    }

    [Fact]
    public async Task CreateClientWithCustomHandlers()
    {
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        var handler = new TestHandler();
        using var client = testableHttpMessageHandler.CreateClient(handler);

        await client.GetAsync("https://httpbin.org/get");

        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/get");
        Assert.True(handler.WasCalled);
    }

    private class TestHandler : DelegatingHandler
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
