using System.Net.Http;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public partial class HttpResponseMessageChecksTests
    {
        [Fact]
        public void HasContentHeader_WhenHttpResponseMessageIsNull_DoesFail()
        {
            HttpResponseMessage sut = null;

            Check.ThatCode(() => Check.That(sut).HasContentHeader("Content-Disposition"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response is null."
                );
        }

        [Fact]
        public void HasContentHeader_WhenContentIsNull_DoesFail()
        {
            using var sut = new HttpResponseMessage();

            Check.ThatCode(() => Check.That(sut).HasContentHeader("Content-Disposition"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content is null."
                );
        }

        [Fact]
        public void HasContentHeader_WhenHeaderIsPresent_DoesNotFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };
            sut.Content.Headers.Add("Content-Disposition", "inline");

            Check.That(sut).HasContentHeader("Content-Disposition");
        }

        [Fact]
        public void HasContentHeader_WhenHeaderIsNotPresent_DoesFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };

            Check.ThatCode(() => Check.That(sut).HasContentHeader("Content-Disposition"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content's headers does not contain the expected header.",
                    "The expected header:",
                    "\t[\"Content-Disposition\"]"
                );
        }

        [Fact]
        public void HasContentHeader_WhenHeaderIsNotPresentAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };


            Check.That(sut).Not.HasContentHeader("Content-Disposition");
        }

        [Fact]
        public void HasContentHeader_WhenHeaderIsPresentAndNotIsUsed_DoesFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };
            sut.Content.Headers.Add("Content-Disposition", "inline");

            Check.ThatCode(() => Check.That(sut).Not.HasContentHeader("Content-Disposition"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content's headers should not contain the forbidden header.",
                    "The forbidden header:",
                    "\t[\"Content-Disposition\"]"
                );
        }
    }
}
