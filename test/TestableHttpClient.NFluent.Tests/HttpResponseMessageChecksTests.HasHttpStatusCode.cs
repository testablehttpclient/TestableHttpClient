using System.Net;
using System.Net.Http;

using NFluent;
using NFluent.Helpers;

using Xunit;

namespace TestableHttpClient.NFluent.Tests
{
    public partial class HttpResponseMessageChecksTests
    {
        [Fact]
        public void HasHttpStatusCode_WhenResponseMessageIsNull_DoesFail()
        {
            HttpResponseMessage httpResponseMessage = null;

            Check.ThatCode(() => Check.That(httpResponseMessage).HasHttpStatusCode(HttpStatusCode.OK))
                .IsAFailingCheckWithMessage(
                "",
                "The checked response is null."
                );
        }

        [Fact]
        public void HasHttpStatusCode_WhenStatusCodeIsCorrect_DoesNotFail()
        {
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            Check.That(httpResponseMessage).HasHttpStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public void HasHttpStatusCode_WhenStatusCodeIsNotCorrect_DoesFail()
        {
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            Check.ThatCode(() => Check.That(httpResponseMessage).HasHttpStatusCode(HttpStatusCode.OK))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's status code is not the expected status code.",
                    "The checked response's status code:",
                    "\t[NotFound]",
                    "The expected status code:",
                    "\t[OK]"
                );
        }

        [Fact]
        public void HasHttpStatusCode_WhenStatusCodeIsNotCorrectAndNotIsUsed_DoesNotFail()
        {
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            Check.That(httpResponseMessage).Not.HasHttpStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public void HasHttpStatusCode_WhenStatusCodeIsCorrectAndNotIsUsed_DoesFail()
        {
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            Check.ThatCode(() => Check.That(httpResponseMessage).Not.HasHttpStatusCode(HttpStatusCode.OK))
                .IsAFailingCheckWithMessage(
                    "",
                    "The checked response's status code should not be the forbidden status code.",
                    "The checked response's status code:",
                    "\t[OK]",
                    "The forbidden status code:",
                    "\t[OK]"
                );
        }
    }
}
