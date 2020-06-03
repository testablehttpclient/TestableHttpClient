using System;
using System.Linq;
using System.Net.Http;
using System.Text;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithHeaderNameAndValue
    {
#nullable disable
        [Fact]
        public void WithHeaderNameAndValue_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader", "someValue"));

            Assert.Equal("check", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(headerName, "someValue"));

            Assert.Equal("headerName", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void WithHeaderNameAndValue_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader", headerValue));

            Assert.Equal("headerValue", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithHeaderNameAndValue_NoRequests_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("someHeader", "someValue"));

            Assert.Equal("Expected at least one request to be made with header 'someHeader' and value 'someValue', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHeaderNameAndValue_RequestWithoutHeaders_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type", "application/json"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Type' and value 'application/json', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHeaderNameAndValue_RequestWithNotMatchingHeaderName_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "test";
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Disposition", "inline"));

            Assert.Equal("Expected at least one request to be made with header 'Content-Disposition' and value 'inline', but no requests were made.", exception.Message);
        }

        [Theory]
        [InlineData("Content-Type", "application/json")]
        [InlineData("Host", "Test")]
        public void WithHeaderNameAndValue_RequestWithNotMatchingHeaderValue_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage(string name, string value)
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "example";
            request.Content = new StringContent("");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader(name, value));

            Assert.Equal($"Expected at least one request to be made with header '{name}' and value '{value}', but no requests were made.", exception.Message);
        }

        [Theory]
        [InlineData("Content-Type", "application/json; charset=utf-8")]
        [InlineData("Host", "Test")]
        public void WithHeaderNameAndValue_RequestWithMatchinHeader_ReturnsHttpRequestMessageAssert(string name, string value)
        {
            var request = new HttpRequestMessage();
            request.Headers.Host = "Test";
            request.Content = new StringContent("", Encoding.UTF8, "application/json");
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithHeader(name, value);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
