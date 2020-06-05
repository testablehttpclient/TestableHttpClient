using System;
using System.Net.Http;
using System.Text;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithJsonContent
    {
#nullable disable
        [Fact]
        public void WithJsonContent_NullChecker_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithJsonContent(null));

            Assert.Equal("check", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithJsonContent_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithJsonContent(null);

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

        [Fact]
        public void WithJsonContent_RequestWithDifferentContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithJsonContent(null));
            Assert.Equal("Expected at least one request to be made with json content 'null', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithJsonContent_RequestWithDifferentContentType_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("null", Encoding.UTF8)
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithJsonContent(null));
            Assert.Equal("Expected at least one request to be made with json content 'null', but no requests were made.", exception.Message);
        }
    }
}
