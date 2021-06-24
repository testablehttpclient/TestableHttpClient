using System;
using System.Threading.Tasks;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public class TestableHttpClientChecksTests
    {
        [Fact]
        public async Task HasMadeRequests_OneRequestMade_Passes()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = sut.CreateClient();

            _ = await client.GetAsync("https://httpbin.com/get").ConfigureAwait(false);

            Check.ThatCode(() => Check.That(sut).HasMadeRequests()).Not.IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequests_NoRequestsMade_Fail()
        {
            using var sut = new TestableHttpMessageHandler();

            Check.ThatCode(() => Check.That(sut).HasMadeRequests()).IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequests_NegatedCheck_Fail()
        {
            using var sut = new TestableHttpMessageHandler();
            Check.ThatCode(() => Check.That(sut).Not.HasMadeRequests()).Throws<InvalidOperationException>().WithMessage("HasMadeRequests can't be used when negated");
        }

        [Fact]
        public void HasMadeRequestsWithExpectedNumberOfRequests_NoRequestsMadeAndZeroRequestsExpeceted_Passes()
        {
            using var sut = new TestableHttpMessageHandler();
            Check.ThatCode(() => Check.That(sut).HasMadeRequests(0)).Not.IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequestsWithExpectedNumberOfRequests_NoRequestsMadeAndOneRequestExpeceted_Passes()
        {
            using var sut = new TestableHttpMessageHandler();
            Check.ThatCode(() => Check.That(sut).HasMadeRequests(1)).IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequestsWithExpectedNumberOfRequests_NegatedCheck_Fail()
        {
            using var sut = new TestableHttpMessageHandler();
            Check.ThatCode(() => Check.That(sut).Not.HasMadeRequests(0)).Throws<InvalidOperationException>().WithMessage("HasMadeRequests can't be used when negated");
        }

        [Fact]
        public async Task HasMadeRequestsTo_OneRequestMade_Passes()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = sut.CreateClient();

            _ = await client.GetAsync("https://httpbin.com/get").ConfigureAwait(false);

            Check.ThatCode(() => Check.That(sut).HasMadeRequestsTo("https://httpbin.com/get")).Not.IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequestsTo_NoRequestsMade_Fail()
        {
            using var sut = new TestableHttpMessageHandler();

            Check.ThatCode(() => Check.That(sut).HasMadeRequestsTo("https://httpbin.com/get")).IsAFailingCheck();
        }

        [Fact]
        public async Task HasMadeRequestsTo_NoMatchingRequestsMade_Fail()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = sut.CreateClient();

            _ = await client.GetAsync("https://httpbin.com/get").ConfigureAwait(false);
            Check.ThatCode(() => Check.That(sut).HasMadeRequestsTo("https://httpbin.com/post")).IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequestsTo_NegatedCheck_Fail()
        {
            using var sut = new TestableHttpMessageHandler();
            Check.ThatCode(() => Check.That(sut).Not.HasMadeRequestsTo("https://httpbin.com/get")).Throws<InvalidOperationException>().WithMessage("HasMadeRequestsTo can't be used when negated");
        }

        [Fact]
        public void HasMadeRequestsToWithExpectedNumberOfRequests_NoRequestsMadeAndZeroExpected_Passes()
        {
            using var sut = new TestableHttpMessageHandler();

            Check.ThatCode(() => Check.That(sut).HasMadeRequestsTo("https://httpbin.com/get", 0)).Not.IsAFailingCheck();
        }

        [Fact]
        public async Task HasMadeRequestsToWithExpectedNumberOfRequests_MatchingNumberOfRequestsMade_Passes()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = sut.CreateClient();

            _ = await client.GetAsync("https://httpbin.com/get").ConfigureAwait(false);
            Check.ThatCode(() => Check.That(sut).HasMadeRequestsTo("https://httpbin.com/get", 1)).Not.IsAFailingCheck();
        }

        [Fact]
        public async Task HasMadeRequestsToWithExpectedNumberOfRequests_NoMatchingNumberOfRequestsMade_Fail()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = sut.CreateClient();

            _ = await client.GetAsync("https://httpbin.com/get").ConfigureAwait(false);
            Check.ThatCode(() => Check.That(sut).HasMadeRequestsTo("https://httpbin.com/get", 2)).IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequestsToWithExpectedNumberOfRequests_NegatedCheck_Fail()
        {
            using var sut = new TestableHttpMessageHandler();
            Check.ThatCode(() => Check.That(sut).Not.HasMadeRequestsTo("https://httpbin.com/get", 0)).Throws<InvalidOperationException>().WithMessage("HasMadeRequestsTo can't be used when negated");
        }
    }
}
