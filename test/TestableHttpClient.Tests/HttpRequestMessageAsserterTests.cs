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
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public void Times_ValueLessThan0_ThrowsArgumentException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<ArgumentException>(() => sut.Times(-1));

            Assert.Equal("count", exception.ParamName);
        }

        [Fact]
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public void Times_NoRequestsAndCount1_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.Times(1));

            Assert.Equal("Expected one request to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public void Times_NoRequestsAndCount2_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.Times(2));

            Assert.Equal("Expected 2 requests to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public void Times_NoRequestsAndCount0_ReturnsHttpRequestMessageAsserter()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());

            var result = sut.Times(0);
            Assert.NotNull(result);
            Assert.IsType<HttpRequestMessageAsserter>(result);
        }

#nullable disable
        [Fact]
        public void With_NullPredicate_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());
            var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFilter(null, "check"));
            Assert.Equal("predicate", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void With_PredicateThatDoesNotMatchAnyRequests_ThrowsAssertionException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, string.Empty));
            Assert.Equal("Expected at least one request to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        public void With_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_ThrowsAssertionException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, "custom check"));
            Assert.Equal("Expected at least one request to be made with custom check, but no requests were made.", exception.Message);
        }

        [Fact]
        public void With_PredicateThatDoesMatchAnyRequests_DoesNotThrow()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            sut.WithFilter(x => x != null, string.Empty);
        }

#nullable disable
        [Fact]
        public void With_WithRequestExpectations_NullPredicate_ThrowsArgumentNullException()
        {
            var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());
            Assert.Throws<ArgumentNullException>("predicate", () => sut.WithFilter(null, 1, "check"));
        }
#nullable restore

        [Fact]
        public void With_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequests_ThrowsAssertionException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, 1, string.Empty));
            Assert.Equal("Expected one request to be made, but no requests were made.", exception.Message);
        }

        [Fact]
        public void With_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_ThrowsAssertionException()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, 1, "custom check"));
            Assert.Equal("Expected one request to be made with custom check, but no requests were made.", exception.Message);
        }

        [Fact]
        public void With_WithRequestExpectation_PredicateThatDoesMatchAnyRequests_DoesNotThrow()
        {
            var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            sut.WithFilter(x => x != null, 1, string.Empty);
        }
    }
}
