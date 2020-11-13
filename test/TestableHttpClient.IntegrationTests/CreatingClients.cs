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
            var testableHttpMessageHandler = new TestableHttpMessageHandler();
            var client = testableHttpMessageHandler.CreateClient();

            await client.GetAsync("https://httpbin.org/get");

#if NETCOREAPP2_1
            testableHttpMessageHandler.ShouldHaveMadeRequests().WithHttpVersion(HttpVersion.Version20);
#else
            testableHttpMessageHandler.ShouldHaveMadeRequests().WithHttpVersion(HttpVersion.Version11);
#endif
        }

        [Fact]
        public async Task CreateClientWithConfiguration()
        {
            var testableHttpMessageHandler = new TestableHttpMessageHandler();
            var client = testableHttpMessageHandler.CreateClient(client => client.DefaultRequestHeaders.Add("test", "test"));

            await client.GetAsync("https://httpbin.org/get");

            testableHttpMessageHandler.ShouldHaveMadeRequests().WithRequestHeader("test", "test");
        }
    }
}
