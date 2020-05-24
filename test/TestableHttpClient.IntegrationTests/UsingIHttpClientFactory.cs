using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Moq;

using Xunit;

namespace TestableHttpClient.IntegrationTests
{
    public class UsingIHttpClientFactory
    {
        [Fact]
        public async Task ConfigureIHttpClientFactoryToUseTestableHttpClient()
        {
            // Create TestableHttpMessageHandler as usual.
            var testableHttpMessageHandler = new TestableHttpMessageHandler();
            testableHttpMessageHandler.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.OK));

            // Create a mock for IHttpClientFactory
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            // Setup the CreateClient method to use the testableHttpMessageHandler
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient(testableHttpMessageHandler));

            // Pass the mocked IHttpClientFactory to the class under test.
            var client = new HttpbinClient(httpClientFactoryMock.Object);
            var result = await client.Get();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get");
        }

        [Fact]
        public async Task ConfigureMultiplehttpClientFactories()
        {
            var testableGithubHandler = new TestableHttpMessageHandler();
            testableGithubHandler.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.OK).WithResponseHeader("Server", "github"));

            var testableHttpBinHandler = new TestableHttpMessageHandler();
            testableHttpBinHandler.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.NotFound).WithResponseHeader("Server", "httpbin"));

            // Create a mock for IHttpClientFactory
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            // Setup the CreateClient method to use the testableHttpMessageHandler
            httpClientFactoryMock.Setup(x => x.CreateClient("github")).Returns(new HttpClient(testableGithubHandler));
            httpClientFactoryMock.Setup(x => x.CreateClient("httpbin")).Returns(new HttpClient(testableHttpBinHandler));

            var httpClientFactory = httpClientFactoryMock.Object;

            var githubClient = httpClientFactory.CreateClient("github");
            await githubClient.GetAsync("https://github.com/api/users");

            var httpbinClient = httpClientFactory.CreateClient("httpbin");
            await httpbinClient.GetAsync("https://httpbin.com/get");

            testableGithubHandler.ShouldHaveMadeRequests();
            testableHttpBinHandler.ShouldHaveMadeRequests();
        }

        private class HttpbinClient
        {
            private readonly IHttpClientFactory _httpClientFactory;
            public HttpbinClient(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }

            public Task<HttpResponseMessage> Get()
            {
                var client = _httpClientFactory.CreateClient();
                return client.GetAsync("https://httpbin.com/get");
            }
        }
    }
}
