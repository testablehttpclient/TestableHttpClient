using System;
using System.Linq;
using System.Net.Http;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public class FluentHttpRequestMessagesChecksTests
    {
#nullable disable
        [Fact]
        public void Constructor_NullHttpRequestMessages_ThrowsArgumentNullException()
        {
            Check.ThatCode(() => new FluentHttpRequestMessagesChecks(null))
                .Throws<ArgumentNullException>()
                .WithProperty(x => x.ParamName, "httpRequestMessages");
        }
#nullable restore

#nullable disable
        [Fact]
        public void With_NullPredicate_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.WithFilter(null, "check"))
                .IsAFailingCheckWithMessage("",
                "The request filter should not be null.");
        }
#nullable restore

        [Fact]
        public void With_PredicateThatDoesNotMatchAnyRequests_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.WithFilter(x => x == null, string.Empty))
                .IsAFailingCheckWithMessage("",
                "Expected at least one request to be made, but no requests were made.");
        }

        [Fact]
        public void With_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_FailsWithMessage()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.WithFilter(x => x == null, "custom check"))
                .IsAFailingCheckWithMessage("",
                "Expected at least one request to be made with custom check, but no requests were made.");
        }

        [Fact]
        public void With_PredicateThatDoesMatchAnyRequests_DoesNotFail()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.WithFilter(x => x != null, string.Empty)).Not.IsAFailingCheck();
        }

#nullable disable
        [Fact]
        public void With_WithRequestExpectations_NullPredicate_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.WithFilter(null, 1, "check"))
                .IsAFailingCheckWithMessage("",
                "The request filter should not be null.");
        }
#nullable restore

        [Fact]
        public void With_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequests_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.WithFilter(x => x == null, 1, string.Empty))
                .IsAFailingCheckWithMessage("",
                "Expected one request to be made, but no requests were made.");
        }

        [Fact]
        public void With_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_FailsWithMessage()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.WithFilter(x => x == null, 1, "custom check"))
                .IsAFailingCheckWithMessage("",
                "Expected one request to be made with custom check, but no requests were made.");
        }

        [Fact]
        public void With_WithRequestExpectation_PredicateThatDoesMatchAnyRequests_DoesNotFail()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.WithFilter(x => x != null, 1, string.Empty)).Not.IsAFailingCheck();
        }

        [Fact]
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public void Times_NegativeNumber_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.Times(-1))
                .IsAFailingCheckWithMessage("",
                "The expected number of requests should not be below zero.",
                "The expected number of requests:",
                "\t[-1]");
        }

        [Fact]
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public void Times_MatchingNumber_DoesNotFail()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.Times(0)).Not.IsAFailingCheck();
        }

        [Fact]
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public void Times_NotMatchingNumber_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.Times(1))
                .IsAFailingCheckWithMessage("",
                "Expected one request to be made, but no requests were made.");
        }
    }
}
