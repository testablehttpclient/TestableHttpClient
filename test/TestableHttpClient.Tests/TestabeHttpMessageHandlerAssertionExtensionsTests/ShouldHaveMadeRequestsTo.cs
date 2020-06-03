using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace TestableHttpClient.Tests.TestabeHttpMessageHandlerAssertionExtensionsTests
{
    public class ShouldHaveMadeRequestsTo
    {
#nullable disable
        [Fact]
        public void ShouldHaveMadeRequestsTo_NullHandler_ThrowsArgumentNullException()
        {
            TestableHttpMessageHandler sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/"));
            Assert.Equal("handler", exception.ParamName);
        }

        [Fact]
        public void ShouldHaveMadeRequestsTo_NullPattern_ThrowsArgumentNullException()
        {
            using var sut = new TestableHttpMessageHandler();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequestsTo(null));
            Assert.Equal("pattern", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void ShouldHaveMadeRequestsTo_WhenNoRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
        {
            using var sut = new TestableHttpMessageHandler();

            Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/"));
        }

        [Fact]
        public async Task ShouldHaveMadeRequestsTo_WhenMatchinRequestsWereMade_ReturnsHttpRequestMessageAsserter()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            _ = await client.GetAsync(new Uri("https://example.com/"));

            var result = sut.ShouldHaveMadeRequestsTo("https://example.com/");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
