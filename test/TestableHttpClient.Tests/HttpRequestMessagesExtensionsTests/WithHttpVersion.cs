using System;
using System.Linq;
using System.Net;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithHttpVersion
    {
#nullable disable
        [Fact]
        public void WithHttpVersion_NullCheck_ThrowsArgumentNulLException()
        {
            IHttpRequestMessagesCheck sut = null;
            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(HttpVersion.Version11));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithHttpVersion_NullHttpVersion_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(null));

            Assert.Equal("httpVersion", exception.ParamName);
        }
#nullable restore

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
