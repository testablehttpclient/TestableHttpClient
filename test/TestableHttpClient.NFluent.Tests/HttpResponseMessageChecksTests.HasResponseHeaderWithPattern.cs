using System.Net.Http;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public partial class HttpResponseMessageChecksTests
    {
        [Fact]
        public void HasResponseHeaderAndValue_WhenHttpResponseMessageIsNull_DoesFail()
        {
            HttpResponseMessage? sut = null;

            Check.ThatCode(() => Check.That(sut).HasResponseHeader("Server", "nginx"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response is null."
                );
        }

        [Theory]
        [InlineData("nginx")]
        [InlineData("*")]
        [InlineData("n*nx")]
        public void HasResponseHeaderAndValue_WhenHeaderIsPresent_DoesNotFail(string pattern)
        {
            using var sut = new HttpResponseMessage();
            sut.Headers.Add("Server", "nginx");

            Check.That(sut).HasResponseHeader("Server", pattern);
        }

        [Fact]
        public void HasResponseHeaderAndValue_WhenHeaderIsNotPresent_DoesFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).HasResponseHeader("Server", "nginx"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's headers does not contain the expected header.",
                    "The expected header:",
                    "\t[\"Server: nginx\"]"
                );
        }

        [Fact]
        public void HasResponseHeaderAndValue_WhenHeaderValueDoesNotMatch_DoesFail()
        {
            using var sut = new HttpResponseMessage();
            sut.Headers.Add("Server", "kestrel");

            Check.ThatCode(() => Check.That(sut).HasResponseHeader("Server", "nginx"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's headers does not contain the expected header.",
                    "The expected header:",
                    "\t[\"Server: nginx\"]"
                );
        }

        [Fact]
        public void HasResponseHeaderAndValue_WhenHeaderIsNotPresentAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage();


            Check.That(sut).Not.HasResponseHeader("Server", "nginx");
        }

        [Fact]
        public void HasResponseHeaderAndValue_WhenHeaderIsPresentAndValueDoesNotMatchAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage();
            sut.Headers.Add("Server", "kestrel");

            Check.That(sut).Not.HasResponseHeader("Server", "nginx");
        }

        [Fact]
        public void HasResponseHeaderAndValue_WhenHeaderIsPresentAndNotIsUsed_DoesFail()
        {
            using var sut = new HttpResponseMessage();
            sut.Headers.Add("Server", "nginx");

            Check.ThatCode(() => Check.That(sut).Not.HasResponseHeader("Server", "nginx"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's headers should not contain the forbidden header.",
                    "The forbidden header:",
                    "\t[\"Server: nginx\"]"
                );
        }
    }
}
