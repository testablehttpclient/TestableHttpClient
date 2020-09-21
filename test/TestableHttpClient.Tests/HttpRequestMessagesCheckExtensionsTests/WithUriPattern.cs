using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests
{
    [Obsolete("Renamed to WithRequestUri")]
    public class WithUriPattern
    {
#nullable disable
        [Fact]
        public void WithUriPattern_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithUriPattern("*"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithUriPattern_NullOrEmptyPattern_ThrowsArgumentNullException(string pattern)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithUriPattern(pattern));

            Assert.Equal("pattern", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithUriPattern_RequestWithMatchingUri_DoesNotThrowException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            sut.WithUriPattern("https://example.com/");
        }

        [Fact]
        public void WithUriPattern_RequestWithMatchingUriAndNegationTurnedOn_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) }, true);

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("https://example.com/"));
            Assert.Equal("Expected no requests to be made with uri pattern 'https://example.com/', but one request was made.", exception.Message);
        }

        [Fact]
        public void WithUriPattern_RequestWithNotMatchingUri_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("https://test.org/"));
            Assert.Equal("Expected at least one request to be made with uri pattern 'https://test.org/', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithUriPattern_RequestWithStarPatternAndNoRequests_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("*"));
            Assert.Equal("Expected at least one request to be made, but no requests were made.", exception.Message);
        }
    }
}
