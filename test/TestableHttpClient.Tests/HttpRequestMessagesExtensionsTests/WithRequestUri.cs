using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithRequestUri
    {
#nullable disable
        [Fact]
        public void WithRequestUri_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri("*"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestUri_NullOrEmptyPattern_ThrowsArgumentNullException(string pattern)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri(pattern));

            Assert.Equal("pattern", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithRequestUri_RequestWithMatchingUri_DoesNotThrowException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            sut.WithRequestUri("https://example.com/");
        }

        [Fact]
        public void WithRequestUri_RequestWithMatchingUriAndNegationTurnedOn_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) }, true);

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://example.com/"));
            Assert.Equal("Expected no requests to be made with uri pattern 'https://example.com/', but one request was made.", exception.Message);
        }

        [Fact]
        public void WithRequestUri_RequestWithNotMatchingUri_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://test.org/"));
            Assert.Equal("Expected at least one request to be made with uri pattern 'https://test.org/', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestUri_RequestWithStarPatternAndNoRequests_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("*"));
            Assert.Equal("Expected at least one request to be made, but no requests were made.", exception.Message);
        }
    }
}
