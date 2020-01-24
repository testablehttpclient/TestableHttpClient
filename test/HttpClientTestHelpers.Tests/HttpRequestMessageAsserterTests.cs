using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace HttpClientTestHelpers.Tests
{
    public class HttpRequestMessageAsserterTests
    {
#nullable disable
        [Fact]
        public void Constructor_NullRequestList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRequestMessageAsserter(null));
        }
#nullable restore

        [Fact]
        public void WithUriPattern_RequestWithMatchingUri_DoesNotThrowException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com")) });

            sut.WithUriPattern("https://example.com");
        }

        [Fact]
        public void WithUriPattern_RequestWithMatchingUriAndNegationTurnedOn_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com")) }, true);

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("https://example.com"));
            Assert.Equal("Expected no requests to be made with uri pattern 'https://example.com', but one request was made.", exception.Message);
        }

        [Fact]
        public void WithUriPattern_RequestWithNotMatchingUri_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com")) });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("https://test.org"));
            Assert.Equal("Expected at least one request to be made with uri pattern 'https://test.org', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithUriPattern_RequestWithStarPatternAndNoRequests_ThrowsHttpRequestMessageassertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithUriPattern("*"));
            Assert.Equal("Expected at least one request to be made, but no requests were made.", exception.Message);
        }

#nullable disable
        [Fact]
        public void WithHttpmethod_NullHttpMethod_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null));

            Assert.Equal("httpMethod", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithHttpmethod_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Get));

            Assert.Equal("Expected at least one request to be made with HTTP Method 'GET', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpmethod_RequestsWithIncorrectHttpMethod_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Post, new Uri("https://example.com")) });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Get));

            Assert.Equal("Expected at least one request to be made with HTTP Method 'GET', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpMethod_RequestsWithCorrectMethod_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com")) });

            var result = sut.WithHttpMethod(HttpMethod.Get);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
