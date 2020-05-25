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

            testableHttpMessageHandler.ShouldHaveMadeRequests().WithHttpVersion(HttpVersion.Version11);
        }

        [Fact]
        public async Task CreateClientWithConfiguration()
        {
            var testableHttpMessageHandler = new TestableHttpMessageHandler();
            var client = testableHttpMessageHandler.CreateClient(client => client.DefaultRequestVersion = HttpVersion.Version20);

            await client.GetAsync("https://httpbin.org/get");

            testableHttpMessageHandler.ShouldHaveMadeRequests().WithHttpVersion(HttpVersion.Version20);
        }
    }
}
