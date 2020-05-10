using System;
using System.Net;
using System.Net.Http;

using NFluent;
using NFluent.Extensibility;

namespace TestableHttpClient.NFluent
{
    public static class HttpResponseMessageChecks
    {
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasHttpStatusCode(this ICheck<HttpResponseMessage?> context, HttpStatusCode expected)
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

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasReasonPhrase(this ICheck<HttpResponseMessage?> context, string expectedReasonPhrase)
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

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasHttpVersion(this ICheck<HttpResponseMessage?> context, Version expected)
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

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasResponseHeader(this ICheck<HttpResponseMessage?> context, string expectedHeader)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Headers, "headers")
                .FailWhen(sut => !sut.Contains(expectedHeader), "The {0} does not contain the expected header.", MessageOption.NoCheckedBlock)
                .DefineExpectedResult(expectedHeader, "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.", MessageOption.NoCheckedBlock)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasResponseHeader(this ICheck<HttpResponseMessage?> context, string expectedHeader, string expectedValue)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Headers, "headers")
                .FailWhen(sut =>
                {
                    if (sut.TryGetValues(expectedHeader, out var values))
                    {
                        var stringValues = string.Join(" ", values);
                        if (StringMatcher.Matches(stringValues, expectedValue))
                        {
                            return false;
                        }
                    }
                    return true;
                }, "The {0} does not contain the expected header.", MessageOption.NoCheckedBlock)
                .DefineExpectedResult($"{expectedHeader}: {expectedValue}", "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.", MessageOption.NoCheckedBlock)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContentHeader(this ICheck<HttpResponseMessage?> context, string expectedHeader)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Content, "content")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Headers, "headers")
                .FailWhen(sut => !sut.Contains(expectedHeader), "The {0} does not contain the expected header.", MessageOption.NoCheckedBlock)
                .DefineExpectedResult(expectedHeader, "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.", MessageOption.NoCheckedBlock)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContentHeader(this ICheck<HttpResponseMessage?> context, string expectedHeader, string expectedValue)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Content, "content")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Headers, "headers")
                .FailWhen(sut =>
                {
                    if (sut.TryGetValues(expectedHeader, out var values))
                    {
                        var stringValues = string.Join(" ", values);
                        if (StringMatcher.Matches(stringValues, expectedValue))
                        {
                            return false;
                        }
                    }
                    return true;
                }, "The {0} does not contain the expected header.", MessageOption.NoCheckedBlock)
                .DefineExpectedResult($"{expectedHeader}: {expectedValue}", "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.", MessageOption.NoCheckedBlock)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContent(this ICheck<HttpResponseMessage?> context)
        {
            ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .FailWhen(sut => sut.Content == null, "The {0} has no content, but content was expected.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock)
                .OnNegate("The {0} has content, but no content was expected.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContent(this ICheck<HttpResponseMessage?> context, string? expectedContent)
        {
            var check = ExtensibilityHelper.BeginCheck(context)
                .SetSutName("response")
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Content, "content");

            if (expectedContent == null)
            {
                check.FailWhen(sut => sut != null, "The {0} should be null.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock)
                    .OnNegate("The {0} should not be null.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock);

            }
            else if (expectedContent.Contains("*"))
            {
                check.FailWhen(sut =>
                {
                    if (sut == null)
                    {
                        return true;
                    }
                    var content = sut.ReadAsStringAsync().Result;

                    return !StringMatcher.Matches(content, expectedContent);
                }, "The {0} does not match the expected pattern.", MessageOption.NoCheckedBlock)
                .DefineExpectedResult(expectedContent, "The expected content pattern:", "The forbidden content pattern:")
                .OnNegate("The {0} should not match the forbidden pattern.", MessageOption.NoCheckedBlock);
            }
            else
            {
                check.FailWhen(sut =>
                {
                    if (sut == null)
                    {
                        return true;
                    }
                    var content = sut.ReadAsStringAsync().Result;

                    return !StringMatcher.Matches(content, expectedContent);
                }, "The {0} should be the expected content.", MessageOption.NoCheckedBlock)
                .DefineExpectedResult(expectedContent, "The expected content:", "The forbidden content:")
                .OnNegate("The {0} should not be the forbidden content.", MessageOption.NoCheckedBlock);
            }
            check.EndCheck();

            return ExtensibilityHelper.BuildCheckLink(context);
        }

    }
}
