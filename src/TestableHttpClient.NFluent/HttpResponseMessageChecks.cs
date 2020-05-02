using System;
using System.Net;
using System.Net.Http;

using NFluent;
using NFluent.Extensibility;
using NFluent.Kernel;

namespace TestableHttpClient.NFluent
{
    public static class HttpResponseMessageChecks
    {
        public static ICheckLink<ICheck<HttpResponseMessage>> HasHttpStatusCode(this ICheck<HttpResponseMessage> context, HttpStatusCode expected)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.StatusCode, "status code")
                .FailWhen(sut => sut != expected, "The {0} is not the expected status code.")
                .DefineExpectedResult(expected, "The expected status code:", "The forbidden status code:")
                .OnNegate("The {0} should not be the forbidden status code.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<HttpResponseMessage>> HasReasonPhrase(this ICheck<HttpResponseMessage> context, string expectedReasonPhrase)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.ReasonPhrase, "reason phrase")
                .FailWhen(sut => sut != expectedReasonPhrase, "The {0} is not the expected reason phrase.")
                .DefineExpectedResult(expectedReasonPhrase, "The expected reason phrase:", "The forbidden reason phrase:")
                .OnNegate("The {0} should not be the forbidden reason phrase.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<HttpResponseMessage>> HasHttpVersion(this ICheck<HttpResponseMessage> context, Version expected)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Version, "version")
                .FailWhen(sut => sut != expected, "The {0} is not the expected version.")
                .DefineExpectedResult(expected, "The expected version:", "The forbidden version:")
                .OnNegate("The {0} should not be the forbidden version.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }
    }
}
