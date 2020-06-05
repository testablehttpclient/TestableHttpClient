using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithRequestHeaderName
    {
#nullable disable
        [Fact]
        public void WithRequestHeader_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeader_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithRequestHeader_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeader_NoMatchingRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage() });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeader_MatchingRequest_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("api-version", "1.0");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithRequestHeader("api-version");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
