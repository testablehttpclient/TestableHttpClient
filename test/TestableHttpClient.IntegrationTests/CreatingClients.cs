using System.Net;
using System.Threading.Tasks;

using Xunit;

namespace TestableHttpClient.IntegrationTests
{
    public class CreatingClients
    {
        [Fact]
        public async Task CreateASimpleHttpClient()
        {
            using var testableHttpMessageHandler = new TestableHttpMessageHandler();
            using var client = testableHttpMessageHandler.CreateClient();

            await client.GetAsync("https://httpbin.org/get").ConfigureAwait(false);

#if NETCOREAPP2_1
            testableHttpMessageHandler.ShouldHaveMadeRequests().WithHttpVersion(HttpVersion.Version20);
#else
            testableHttpMessageHandler.ShouldHaveMadeRequests().WithHttpVersion(HttpVersion.Version11);
#endif
        }

        [Fact]
        public async Task CreateClientWithConfiguration()
        {
            using var testableHttpMessageHandler = new TestableHttpMessageHandler();
            using var client = testableHttpMessageHandler.CreateClient(client => client.DefaultRequestHeaders.Add("test", "test"));

            await client.GetAsync("https://httpbin.org/get").ConfigureAwait(false);

            testableHttpMessageHandler.ShouldHaveMadeRequests().WithRequestHeader("test", "test");
        }
    }
}
