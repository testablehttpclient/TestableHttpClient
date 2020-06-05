using System;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithContentHeaderNameAndValue
    {
#nullable disable
        [Fact]
        public void WihtContentHeaderNameAndValue_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", "someValue"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithContentHeaderNameAndValue_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithContentHeaderNameAndValue_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("someHeader", "someValue"));

            Assert.Equal("Expected at least one request to be made with content header 'someHeader' and value 'someValue', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithoutHeaders_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithNotMatchingHeaderName_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithNotMatchingHeaderValue_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with content header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContentHeaderNameAndValue_RequestWithMatchingHeader_ReturnsHttpRequestMessageAssert()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithContentHeader("Content-Type", "application/json; charset=utf-8");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
