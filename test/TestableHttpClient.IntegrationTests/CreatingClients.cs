namespace TestableHttpClient.IntegrationTests;

public class CreatingClients
{
    [Fact]
    public async Task CreateASimpleHttpClient()
    {
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        using var client = testableHttpMessageHandler.CreateClient();

        await client.GetAsync("https://httpbin.org/get");

        testableHttpMessageHandler.ShouldHaveMadeRequests().WithHttpVersion(HttpVersion.Version11);
    }

    [Fact]
    public async Task CreateClientWithConfiguration()
    {
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        using var client = testableHttpMessageHandler.CreateClient(client => client.DefaultRequestHeaders.Add("test", "test"));

        await client.GetAsync("https://httpbin.org/get");

        testableHttpMessageHandler.ShouldHaveMadeRequests().WithRequestHeader("test", "test");
    }
}
