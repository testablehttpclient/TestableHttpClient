using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace TestableHttpClient.IntegrationTests
{
    public class ConfigureResponses
    {
        [Fact]
        public async Task UsingTestHandler_WithoutSettingUpResponse_Returns200OKWithoutContent()
        {
            using var testHandler = new TestableHttpMessageHandler();

            using var httpClient = new HttpClient(testHandler);
            var result = await httpClient.GetAsync("http://httpbin.org/status/200");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task UsingTestHandlerWithCustomResponse_ReturnsCustomResponse()
        {
            using var testHandler = new TestableHttpMessageHandler();
            using var response = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent("HttpClient testing is easy", Encoding.UTF8, "text/plain")
            };
            testHandler.RespondWith(response);

            using var httpClient = new HttpClient(testHandler);
            var result = await httpClient.GetAsync("http://httpbin.org/status/201");

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task UsingTestHandlerWithCustomResponseUsingBuilder_ReturnsCustomResponse()
        {
            using var testHandler = new TestableHttpMessageHandler();
            testHandler.RespondWith(response =>
            {
                response.WithHttpStatusCode(HttpStatusCode.Created)
                        .WithStringContent("HttpClient testing is easy");
            });

            using var httpClient = new HttpClient(testHandler);
            var result = await httpClient.GetAsync("http://httpbin.org/status/201");

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task UsingTestHandlerWithMultipleCustomRepsonse_ReturnsLastCustomResponse()
        {
            using var testHandler = new TestableHttpMessageHandler();
            testHandler.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.Created).WithStringContent("HttpClient testing is easy"));
            testHandler.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.NotFound).WithJsonContent("Not Found"));

            using var httpClient = new HttpClient(testHandler);
            var result = await httpClient.GetAsync("http://httpbin.org/status/201");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("\"Not Found\"", await result.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task UsingTestHandlerWithCustomResponse_AlwaysReturnsSameCustomResponse()
        {
            using var testHandler = new TestableHttpMessageHandler();
            testHandler.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.Created).WithStringContent("HttpClient testing is easy"));

            using var httpClient = new HttpClient(testHandler);
            var urls = new[]
            {
                "http://httpbin.org/status/200",
                "http://httpbin.org/status/201",
                "http://httpbin.org/status/400",
                "http://httpbin.org/status/401",
                "http://httpbin.org/status/503",
            };

            foreach (var url in urls)
            {
                var result = await httpClient.GetAsync(url);

                Assert.Equal(HttpStatusCode.Created, result.StatusCode);
                Assert.Equal("HttpClient testing is easy", await result.Content.ReadAsStringAsync());
            }
        }

        [Fact]
        public async Task SimulateTimeout_WillThrowExceptionSimulatingTheTimeout()
        {
            using var testHandler = new TestableHttpMessageHandler();
            testHandler.SimulateTimeout();

            using var httpClient = new HttpClient(testHandler);
            await Assert.ThrowsAsync<TaskCanceledException>(() => httpClient.GetAsync("https://httpbin.org/delay/500"));
        }
    }
}
