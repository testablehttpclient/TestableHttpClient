using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithHeaderName
    {
#nullable disable
        [Fact]
        public void WithHeader_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeader_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithHeader_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHeader_NoMatchingRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage() });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Theory]
        [InlineData("Host")]
        [InlineData("Content-Type")]
        public void WithHeader_MatchingRequest_ReturnsHttpRequestMessageAsserter(string headerName)
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "host";
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithHeader(headerName);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
