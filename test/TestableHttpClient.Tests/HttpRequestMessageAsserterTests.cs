using System;
using System.Linq;
using System.Net.Http;

using Xunit;

namespace TestableHttpClient.Tests
{
    public class HttpRequestMessageAsserterTests
    {
#nullable disable
        [Fact]
        public void Constructor_NullRequestList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRequestMessageAsserter(null));
        }
#nullable restore

        [Fact]
        public void Times_ValueLessThan0_ThrowsArgumentException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentException>(() => sut.Times(-1));

            Assert.Equal("count", exception.ParamName);
        }

        [Fact]
        public void Times_NoRequestsAndCount1_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.Times(1));

            Assert.Equal("Expected one request to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        public void Times_NoRequestsAndCount2_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.Times(2));

            Assert.Equal("Expected 2 requests to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        public void Times_NoRequestsAndCount0_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var result = sut.Times(0);
            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }
    }
}
