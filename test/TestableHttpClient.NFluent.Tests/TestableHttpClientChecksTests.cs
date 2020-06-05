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

            _ = await client.GetAsync("https://httpbin.com/get");

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
        public async Task HasMadeRequestsTo_OneRequestMade_Passes()
        {
            using var sut = new TestableHttpMessageHandler();
            using var client = sut.CreateClient();

            _ = await client.GetAsync("https://httpbin.com/get");

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

            _ = await client.GetAsync("https://httpbin.com/get");
            Check.ThatCode(() => Check.That(sut).HasMadeRequestsTo("https://httpbin.com/post")).IsAFailingCheck();
        }

        [Fact]
        public void HasMadeRequestsTo_NegatedCheck_Fail()
        {
            using var sut = new TestableHttpMessageHandler();
            Check.ThatCode(() => Check.That(sut).Not.HasMadeRequestsTo("https://httpbin.com/get")).Throws<InvalidOperationException>().WithMessage("HasMadeRequestsTo can't be used when negated");
        }
    }
}
