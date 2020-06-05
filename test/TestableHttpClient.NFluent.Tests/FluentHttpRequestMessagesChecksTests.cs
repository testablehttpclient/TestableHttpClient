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
        public void When_NullPredicate_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.With(null, "check"))
                .IsAFailingCheckWithMessage("",
                "The predicate should not be null.");
        }
#nullable restore

        [Fact]
        public void When_PredicateThatDoesNotMatchAnyRequests_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.With(x => x == null, string.Empty))
                .IsAFailingCheckWithMessage("",
                "Expected at least one request to be made, but no requests were made.");
        }

        [Fact]
        public void When_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_FailsWithMessage()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.With(x => x == null, "custom check"))
                .IsAFailingCheckWithMessage("",
                "Expected at least one request to be made with custom check, but no requests were made.");
        }

        [Fact]
        public void When_PredicateThatDoesMatchAnyRequests_DoesNotFail()
        {
            var sut = new FluentHttpRequestMessagesChecks(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

            Check.ThatCode(() => sut.With(x => x != null, string.Empty)).Not.IsAFailingCheck();
        }

        [Fact]
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
        public void Times_MatchingNumber_DoesNotFail()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.Times(0)).Not.IsAFailingCheck();
        }

        [Fact]
        public void Times_NotMatchingNumber_Fails()
        {
            var sut = new FluentHttpRequestMessagesChecks(Enumerable.Empty<HttpRequestMessage>());
            Check.ThatCode(() => sut.Times(1))
                .IsAFailingCheckWithMessage("",
                "Expected one request to be made, but no requests were made.");
        }
    }
}
