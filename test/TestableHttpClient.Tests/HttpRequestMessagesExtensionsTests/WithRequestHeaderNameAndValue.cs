using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithRequestHeaderNameAndValue
    {
#nullable disable
        [Fact]
        public void WithRequestHeaderNameAndValue_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", "someValue"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeaderNameAndValue_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithRequestHeaderNameAndValue_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithRequestHeaderNameAndValue_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("someHeader", "someValue"));

            Assert.Equal("Expected at least one request to be made with request header 'someHeader' and value 'someValue', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithoutHeaders_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version", "1.0"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version' and value '1.0', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithNotMatchingHeaderName_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("no-api-version", "1.0");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version", "1.0"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version' and value '1.0', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithNotMatchingHeaderValue_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("api-version", "unknown");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("api-version", "1.0"));

            Assert.Equal("Expected at least one request to be made with request header 'api-version' and value '1.0', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithRequestHeaderNameAndValue_RequestWithMatchinHeader_ReturnsHttpRequestMessageAssert()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("api-version", "1.0");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithRequestHeader("api-version", "1.0");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
