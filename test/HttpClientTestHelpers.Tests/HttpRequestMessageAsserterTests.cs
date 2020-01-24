using System;
using System.Linq;
using System.Net;
using System.Net.Http;

using Xunit;

namespace HttpClientTestHelpers.Tests
{
    public class HttpRequestMessageAsserterTests
    {
        [Fact]
        public void Constructor_NullRequestList_ThrowsArgumentNullException()
        {
#nullable disable
            Assert.Throws<ArgumentNullException>(() => new HttpRequestMessageAsserter(null));
#nullable restore
        }

#nullable disable
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

        [Fact]
        public void WithHttpMethod_NullHttpMethod_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

#nullable disable
            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null));
#nullable restore

            Assert.Equal("httpMethod", exception.ParamName);
        }

        [Fact]
        public void WithHttpMethod_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Get));

            Assert.Equal("Expected at least one request to be made with HTTP Method 'GET', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpMethod_RequestsWithIncorrectHttpMethod_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
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

        [Fact]
        public void WithHttpVersion_NullHttpVersion_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

#nullable disable
            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(null));
#nullable restore

            Assert.Equal("httpVersion", exception.ParamName);
        }

        [Fact]
        public void WithHttpVersion_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpVersion(HttpVersion.Version11));

            Assert.Equal("Expected at least one request to be made with HTTP Version '1.1', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpVersion_RequestsWithIncorrectHttpVersion_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage { Version = HttpVersion.Version20 } });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpVersion(HttpVersion.Version11));

            Assert.Equal("Expected at least one request to be made with HTTP Version '1.1', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpVersion_RequestsWithCorrectVersion_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage { Version = HttpVersion.Version11 } });

            var result = sut.WithHttpVersion(HttpVersion.Version11);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
