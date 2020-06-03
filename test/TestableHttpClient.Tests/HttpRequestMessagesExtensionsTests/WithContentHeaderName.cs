using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithContentHeaderName
    {
#nullable disable
        [Fact]
        public void WithContentHeader_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeader_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName));

            Assert.Equal("headerName", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithContentHeader_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeader_NoMatchingRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage() });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeader_MatchingRequest_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithContentHeader("Content-Type");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
