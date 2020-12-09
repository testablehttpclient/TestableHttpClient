using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace TestableHttpClient.Tests.TestabeHttpMessageHandlerAssertionExtensionsTests
{
    [Obsolete]
    public class ShouldNotHaveMadeRequestsTo
    {
#nullable disable
        [Fact]
        public void ShouldNotHaveMadeRequestsTo_NullHandler_ThrowsArgumentNullException()
        {
            TestableHttpMessageHandler sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotHaveMadeRequestsTo("https://example.com/"));
            Assert.Equal("handler", exception.ParamName);
        }

        [Fact]
        public void ShouldNotHaveMadeRequestsTo_NullPattern_ThrowsArgumentNullException()
        {
            using var sut = new TestableHttpMessageHandler();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotHaveMadeRequestsTo(null));
            Assert.Equal("pattern", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void ShouldNotHaveMadeRequestsTo_WhenNoRequestsWereMade_DoesNotThrowExceptions()
        {
            using var sut = new TestableHttpMessageHandler();

            sut.ShouldNotHaveMadeRequestsTo("https://example.com/");
        }

        [Fact]
        public async Task ShouldNotHaveMadeRequestsTo_WhenMatchinRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = new HttpClient(sut);

            _ = await client.GetAsync(new Uri("https://example.com/"));

            Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldNotHaveMadeRequestsTo("https://example.com/"));
        }
    }
}
