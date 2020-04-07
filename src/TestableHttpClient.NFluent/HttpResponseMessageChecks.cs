using System;
using System.Net;
using System.Net.Http;

using NFluent;
using NFluent.Extensibility;

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
