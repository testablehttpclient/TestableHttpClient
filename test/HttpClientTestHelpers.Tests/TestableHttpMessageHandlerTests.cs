using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace HttpClientTestHelpers.Tests
{
    public class TestableHttpMessageHandlerTests
    {
        [Fact]
        public async Task SendAsync_WhenRequestsAreMade_LogsRequests()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com");

            _ = await client.SendAsync(request);

            Assert.Contains(request, sut.Requests);
        }

        [Fact]
        public async Task SendAsync_WhenMultipleRequestsAreMade_AllRequestsAreLogged()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);
            using var request1 = new HttpRequestMessage(HttpMethod.Get, "https://example1.com");
            using var request2 = new HttpRequestMessage(HttpMethod.Post, "https://example2.com");
            using var request3 = new HttpRequestMessage(HttpMethod.Delete, "https://example3.com");
            using var request4 = new HttpRequestMessage(HttpMethod.Head, "https://example4.com");

            _ = await client.SendAsync(request1);
            _ = await client.SendAsync(request2);
            _ = await client.SendAsync(request3);
            _ = await client.SendAsync(request4);

            Assert.Equal(new[] { request1, request2, request3, request4 }, sut.Requests);
        }

        [Fact]
        public async Task SendAsync_ByDefault_ReturnsHttpStatusCodeOK()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            var result = await client.GetAsync(new Uri("https://example.com"));

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WhenRespondWithIsSet_SetRespondIsUsed()
        {
            using var sut = new TestableHttpMessageHandler();
            using var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            sut.RespondWith(response);
            using var client = new HttpClient(sut);

            var result = await client.GetAsync(new Uri("https://example.com"));

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Same(response, result);
        }

        [Fact]
        public void ShouldHaveMadeRequests_WhenNoRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
        {
            using var sut = new TestableHttpMessageHandler();

            Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequests());
        }

        [Fact]
        public async Task ShouldHaveMadeRequests_WhenRequestsWereMade_DoesNotThrowExceptions()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            _ = await client.GetAsync(new Uri("https://example.com"));

            sut.ShouldHaveMadeRequests();
        }

        [Fact]
        public void ShouldHaveMadeRequestsTo_WhenNoRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
        {
            using var sut = new TestableHttpMessageHandler();

            Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com"));
        }

        [Fact]
        public async Task ShouldHaveMadeRequestsTo_WhenMatchinRequestsWereMade_ReturnsHttpRequestMessageAsserter()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            _ = await client.GetAsync(new Uri("https://example.com"));

            var result = sut.ShouldHaveMadeRequestsTo("https://example.com");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

        [Fact]
        public void ShouldNotHaveMadeRequests_WhenNoRequestsWereMade_DoesNotThrowExceptions()
        {
            using var sut = new TestableHttpMessageHandler();

            sut.ShouldNotHaveMadeRequests();
        }

        [Fact]
        public async Task ShouldNotHaveMadeRequests_WhenASingleRequestWasMade_ThrowsHttpRequestMessageAssertionException()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            _ = await client.GetAsync(new Uri("https://example.com"));

            var result = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldNotHaveMadeRequests());
            Assert.Equal("Expected no requests to be made, but one request was made.", result.Message);
        }

        [Fact]
        public async Task ShouldNotHaveMadeRequests_WhenMultipleRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            _ = await client.GetAsync(new Uri("https://example.com"));
            _ = await client.GetAsync(new Uri("https://example.com"));

            var result = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldNotHaveMadeRequests());
            Assert.Equal("Expected no requests to be made, but 2 requests were made.", result.Message);
        }

        [Fact]
        public void ShouldNotHaveMadeRequestsTo_WhenNoRequestsWereMade_ReturnsHttpRequestMessageAsserter()
        {
            using var sut = new TestableHttpMessageHandler();

            var result = sut.ShouldNotHaveMadeRequestsTo("https://example.com");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

        [Fact]
        public async Task ShouldNotHaveMadeRequestsTo_WhenMatchinRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            _ = await client.GetAsync(new Uri("https://example.com"));

            Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldNotHaveMadeRequestsTo("https://example.com"));
        }

#nullable disable
        [Fact]
        public void ShouldHaveMadeRequestsTo_WhenGivenPatternIsNull_ThrowsArgumentNullException()
        {
            using var sut = new TestableHttpMessageHandler();

            Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequestsTo(null));
        }

        [Fact]
        public void ShouldNotHaveMadeRequestsTo_WhenGivenPatternIsNull_ThrowsArgumentNullException()
        {
            using var sut = new TestableHttpMessageHandler();

            Assert.Throws<ArgumentNullException>(() => sut.ShouldNotHaveMadeRequestsTo(null));
        }

        [Fact]
        public void RespondWith_NullValue_ThrowsArgumentNullException()
        {
            using var sut = new TestableHttpMessageHandler();
            var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(null));
            Assert.Equal("httpResponseMessage", exception.ParamName);
        }
#nullable restore

        [Fact]
        public async Task SimulateTimout_WhenRequestIsMade_ThrowsTaskCancelationExceptionWithOperationCanceledMessage()
        {
            using var sut = new TestableHttpMessageHandler();
            sut.SimulateTimeout();
            using var client = new HttpClient(sut);

            var exception = await Assert.ThrowsAsync<TaskCanceledException>(() => client.GetAsync(new Uri("https://example.com")));
            Assert.Equal(new OperationCanceledException().Message, exception.Message);
        }
    }
}
