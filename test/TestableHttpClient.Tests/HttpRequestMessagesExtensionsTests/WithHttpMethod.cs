using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithHttpMethod
    {
#nullable disable
        [Fact]
        public void WithHttpMethod_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(HttpMethod.Get));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithHttpMethod_NullHttpMethod_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null));

            Assert.Equal("httpMethod", exception.ParamName);
        }
#nullable restore

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
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Post, new Uri("https://example.com/")) });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Get));

            Assert.Equal("Expected at least one request to be made with HTTP Method 'GET', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithHttpMethod_RequestsWithCorrectMethod_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/")) });

            var result = sut.WithHttpMethod(HttpMethod.Get);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
