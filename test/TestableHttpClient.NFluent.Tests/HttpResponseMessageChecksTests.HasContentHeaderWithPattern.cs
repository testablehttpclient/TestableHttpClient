using System.Net.Http;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public partial class HttpResponseMessageChecksTests
    {
        [Fact]
        public void HasContentHeaderAndValue_WhenHttpResponseMessageIsNull_DoesFail()
        {
            HttpResponseMessage? sut = null;

            Check.ThatCode(() => Check.That(sut).HasContentHeader("Content-Disposition", "inline"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response is null."
                );
        }

        [Fact]
        public void HasContentHeaderAndValue_WhenContentIsNull_DoesFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = null
            };

            Check.ThatCode(() => Check.That(sut).HasContentHeader("Content-Disposition", "inline"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content is null."
                );
        }

        [Theory]
        [InlineData("inline")]
        [InlineData("*")]
        [InlineData("i*e")]
        public void HasContentHeaderAndValue_WhenHeaderIsPresent_DoesNotFail(string pattern)
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };
            sut.Content.Headers.Add("Content-Disposition", "inline");

            Check.That(sut).HasContentHeader("Content-Disposition", pattern);
        }

        [Fact]
        public void HasContentHeaderAndValue_WhenHeaderIsNotPresent_DoesFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };

            Check.ThatCode(() => Check.That(sut).HasContentHeader("Content-Disposition", "inline"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content's headers does not contain the expected header.",
                    "The expected header:",
                    "\t[\"Content-Disposition: inline\"]"
                );
        }

        [Fact]
        public void HasContentHeaderAndValue_WhenHeaderValueDoesNotMatch_DoesFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };
            sut.Content.Headers.Add("Content-Disposition", "attachment");

            Check.ThatCode(() => Check.That(sut).HasContentHeader("Content-Disposition", "inline"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content's headers does not contain the expected header.",
                    "The expected header:",
                    "\t[\"Content-Disposition: inline\"]"
                );
        }

        [Fact]
        public void HasContentHeaderAndValue_WhenHeaderIsNotPresentAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };


            Check.That(sut).Not.HasContentHeader("Content-Disposition", "inline");
        }

        [Fact]
        public void HasContentHeaderAndValue_WhenHeaderIsPresentAndValueDoesNotMatchAndNotIsUsed_DoesNotFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };
            sut.Content.Headers.Add("Content-Disposition", "attachment");

            Check.That(sut).Not.HasContentHeader("Content-Disposition", "inline");
        }

        [Fact]
        public void HasContentHeaderAndValue_WhenHeaderIsPresentAndNotIsUsed_DoesFail()
        {
            using var sut = new HttpResponseMessage()
            {
                Content = new StringContent("")
            };
            sut.Content.Headers.Add("Content-Disposition", "inline");

            Check.ThatCode(() => Check.That(sut).Not.HasContentHeader("Content-Disposition", "inline"))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's content's headers should not contain the forbidden header.",
                    "The forbidden header:",
                    "\t[\"Content-Disposition: inline\"]"
                );
        }
    }
}
