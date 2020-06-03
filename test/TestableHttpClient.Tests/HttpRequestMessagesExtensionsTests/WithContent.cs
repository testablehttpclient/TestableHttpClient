using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests.HttpRequestMessagesExtensionsTests
{
    public class WithContent
    {
#nullable disable
        [Fact]
        public void WithContent_NullCheck_ThrowsArgumentNullException()
        {
            IHttpRequestMessagesCheck sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent("*"));

            Assert.Equal("check", exception.ParamName);
        }

        [Fact]
        public void WithContent_NullPattern_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent(null));

            Assert.Equal("pattern", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void WithContent_RequestWithNotMatchingContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent("some content"));

            Assert.Equal("Expected at least one request to be made with content 'some content', but no requests were made.", exception.Message);
        }

        [Fact]
        public void WithContent_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
        {
            var request = new HttpRequestMessage
            {
                Content = new StringContent("")
            };
            var sut = new HttpRequestMessageAsserter(new[] { request });

            var result = sut.WithContent("");

            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
