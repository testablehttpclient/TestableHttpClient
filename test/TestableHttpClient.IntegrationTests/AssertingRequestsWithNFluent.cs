using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using NFluent;
using NFluent.Helpers;

using TestableHttpClient.NFluent;

using Xunit;

namespace TestableHttpClient.IntegrationTests
{
    public class AssertingRequestsWithNFluent
    {
        [Fact]
        public void BasicAssertionsWhenNoCallsWereMade()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests()).IsAFailingCheck();
        }

        [Fact]
        public async Task WhenAssertingCallsAreNotMade_AndCallsWereMade_AssertionExceptionIsThrow()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            Check.That(testHandler).HasMadeRequests();
        }


        [Fact]
        public async Task AssertingCallsAreNotMadeToSpecificUri()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/get");
        }

        [Fact]
        public async Task AssertingCallsAreMadeToSpecificUriPattern()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            Check.That(testHandler).HasMadeRequestsTo("https://*");
            Check.That(testHandler).HasMadeRequestsTo("https://*.org/get");
            Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/*");
            Check.That(testHandler).HasMadeRequestsTo("*://httpbin.org/get");
            Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/get");
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("http://httpbin.org/get")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("https://*/post")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("https://example.org/")).IsAFailingCheck();
        }

        [Fact]
        public async Task AssertingCallsUsingUriPattern()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            Check.That(testHandler).HasMadeRequests().WithRequestUri("https://*");
            Check.That(testHandler).HasMadeRequests().WithRequestUri("https://*.org/get");
            Check.That(testHandler).HasMadeRequests().WithRequestUri("https://httpbin.org/*");
            Check.That(testHandler).HasMadeRequests().WithRequestUri("*://httpbin.org/get");
            Check.That(testHandler).HasMadeRequests().WithRequestUri("https://httpbin.org/get");
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("http://httpbin.org/get")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("https://httpbin.org/")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("https://*/post")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("https://example.org/")).IsAFailingCheck();
        }

        [Fact]
        public async Task ChainUriPatternAssertions()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.GetAsync("https://httpbin.org/get");

            Check.That(testHandler).HasMadeRequestsTo("https://*")
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

            Check.That(testHandler).HasMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Get);
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Post)).IsAFailingCheck();
            Check.That(testHandler).HasMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Post);
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Get)).IsAFailingCheck();
        }

        [Fact]
        public async Task AssertingRequestHeaders()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);
            client.DefaultRequestHeaders.Add("api-version", "1.0");
            _ = await client.GetAsync("https://httpbin.org/get");

            Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version");
            Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "1.0");
            Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "1*");

            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestHeader("my-version")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "1")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "2*")).IsAFailingCheck();
        }

        [Fact]
        public async Task AssertingContentHeaders()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("", Encoding.UTF8, "application/json"));

            Check.That(testHandler).HasMadeRequests().WithContentHeader("content-type");
            Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type");
            Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "application/json; charset=utf-8");
            Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "application/json*");
            Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "*charset=utf-8");

            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Disposition")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "application/json")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "*=utf-16")).IsAFailingCheck();
        }

        [Fact]
        public async Task AssertingContent()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("my special content"));

            Check.That(testHandler).HasMadeRequests().WithContent("my special content");
            Check.That(testHandler).HasMadeRequests().WithContent("my*content");
            Check.That(testHandler).HasMadeRequests().WithContent("*");

            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContent("")).IsAFailingCheck();
            Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContent("my")).IsAFailingCheck();
        }

        [Fact]
        public async Task AssertJsonContent()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("{}", Encoding.UTF8, "application/json"));

            Check.That(testHandler).HasMadeRequests().WithJsonContent(new { });
        }

        [Fact]
        public async Task CustomAssertions()
        {
            var testHandler = new TestableHttpMessageHandler();
            var client = new HttpClient(testHandler);

            _ = await client.PostAsync("https://httpbin.org/post", new StringContent("", Encoding.UTF8, "application/json"));

            Check.That(testHandler).HasMadeRequests().WithFilter(x => x.HasContentHeader("Content-Type", "application/json") || x.HasContentHeader("Content-Type", "application/json; *"), "");
        }
    }
}
