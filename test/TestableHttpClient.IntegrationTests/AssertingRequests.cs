using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace TestableHttpClient.IntegrationTests
{
    public class AssertingRequests
    {
        [Fact]
        public void BasicAssertionsWhenNoCallsWereMade()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            testHandler.ShouldNotHaveMadeRequests();
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests());
        }

        [Fact]
        public async Task WhenAssertingCallsAreNotMade_AndCallsWereMade_AssertionExceptionIsThrow()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            testHandler.ShouldHaveMadeRequests();
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldNotHaveMadeRequests());
        }


        [Fact]
        public async Task AssertingCallsAreNotMadeToSpecificUri()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            testHandler.ShouldNotHaveMadeRequestsTo("https://example.org");
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldNotHaveMadeRequestsTo("https://httpbin.org/get"));
        }

        [Fact]
        public async Task AssertingCallsAreMadeToSpecificUriPattern()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            testHandler.ShouldHaveMadeRequestsTo("https://*");
            testHandler.ShouldHaveMadeRequestsTo("https://*.org/get");
            testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/*");
            testHandler.ShouldHaveMadeRequestsTo("*://httpbin.org/get");
            testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/get");
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("http://httpbin.org/get"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("https://*/post"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("https://example.org/"));
        }

        [Fact]
        public async Task AssertingCallsUsingUriPattern()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            testHandler.ShouldHaveMadeRequests().WithRequestUri("https://*");
            testHandler.ShouldHaveMadeRequests().WithRequestUri("https://*.org/get");
            testHandler.ShouldHaveMadeRequests().WithRequestUri("https://httpbin.org/*");
            testHandler.ShouldHaveMadeRequests().WithRequestUri("*://httpbin.org/get");
            testHandler.ShouldHaveMadeRequests().WithRequestUri("https://httpbin.org/get");
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("http://httpbin.org/get"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("https://httpbin.org/"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("https://*/post"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("https://example.org/"));
        }

        [Fact]
        public async Task ChainUriPatternAssertions()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            testHandler.ShouldHaveMadeRequestsTo("https://*")
                       .WithRequestUri("*://httpbin.org/*")
                       .WithRequestUri("*/get");
        }

        [Fact]
        public async Task AssertingHttpMethods()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");
            _ = await client.PostAsync("https://httpbin.org/post", new StringContent(""));

            testHandler.ShouldHaveMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Get);
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Post));
            testHandler.ShouldHaveMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Post);
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Get));
        }

        [Fact]
        public async Task AssertingRequestHeaders()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            _ = await client.GetAsync("https://httpbin.org/get");

            testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version");
            testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "1.0");
            testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "1*");

            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestHeader("my-version"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "1"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "2*"));
        }

        [Fact]
        public async Task AssertingContentHeaders()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("", Encoding.UTF8, "application/json"));

            testHandler.ShouldHaveMadeRequests().WithContentHeader("content-type");
            testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type");
            testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "application/json; charset=utf-8");
            testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "application/json*");
            testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "*charset=utf-8");

            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Disposition"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "application/json"));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "*=utf-16"));
        }

        [Fact]
        public async Task AssertingContent()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("my special content"));

            testHandler.ShouldHaveMadeRequests().WithContent("my special content");
            testHandler.ShouldHaveMadeRequests().WithContent("my*content");
            testHandler.ShouldHaveMadeRequests().WithContent("*");

            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContent(""));
            Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContent("my"));
        }

        [Fact]
        public async Task AssertJsonContent()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("{}", Encoding.UTF8, "application/json"));

            testHandler.ShouldHaveMadeRequests().WithJsonContent(new { });
        }

        [Fact]
        public async Task CustomAssertions()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("", Encoding.UTF8, "application/json"));

            testHandler.ShouldHaveMadeRequests().With(x => x.HasContentHeader("Content-Type", "application/json") || x.HasContentHeader("Content-Type", "application/json; *"), "");
        }
    }
}
