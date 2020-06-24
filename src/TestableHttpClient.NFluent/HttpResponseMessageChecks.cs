using System;
using System.Linq;
using System.Net;
using System.Net.Http;

using NFluent;
using NFluent.Extensibility;

using TestableHttpClient.Utils;

namespace TestableHttpClient.NFluent
{
    /// <summary>
    /// A set of NFluent checks to validate <see cref="HttpResponseMessage"/>s.
    /// </summary>
    public static class HttpResponseMessageChecks
    {
        /// <summary>
        /// Verify that the response has a specific status code.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedStatusCode">The expected <see cref="HttpStatusCode"/>.</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasHttpStatusCode(this ICheck<HttpResponseMessage?> check, HttpStatusCode expectedStatusCode)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .CheckSutAttributes(sut => sut.StatusCode, "status code")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailWhen(sut => sut != expectedStatusCode, "The {0} is not the expected status code.")
                .DefineExpectedResult(expectedStatusCode, "The expected status code:", "The forbidden status code:")
                .OnNegate("The {0} should not be the forbidden status code.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has a specific reason phrase.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedReasonPhrase">The expected reason phrase.</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasReasonPhrase(this ICheck<HttpResponseMessage?> check, string expectedReasonPhrase)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .CheckSutAttributes(sut => sut.ReasonPhrase, "reason phrase")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailWhen(sut => sut != expectedReasonPhrase, "The {0} is not the expected reason phrase.")
                .DefineExpectedResult(expectedReasonPhrase, "The expected reason phrase:", "The forbidden reason phrase:")
                .OnNegate("The {0} should not be the forbidden reason phrase.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has a specific HTTP version.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedHttpVersion">The expected <see cref="HttpVersion"/>.</param>
        /// <returns>A check link./returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasHttpVersion(this ICheck<HttpResponseMessage?> check, Version expectedHttpVersion)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .CheckSutAttributes(sut => sut.Version, "version")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailWhen(sut => sut != expectedHttpVersion, "The {0} is not the expected version.")
                .DefineExpectedResult(expectedHttpVersion, "The expected version:", "The forbidden version:")
                .OnNegate("The {0} should not be the forbidden version.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has a response header with a given name.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedHeader">The name of the response header.</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasResponseHeader(this ICheck<HttpResponseMessage?> check, string expectedHeader)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .CheckSutAttributes(sut => sut.Headers.Select(x => x.Key).ToArray(), "headers")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailWhen(sut => !sut.Contains(expectedHeader), "The {0} does not contain the expected header.")
                .DefineExpectedResult(expectedHeader, "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has a response header with a given name and value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedHeader">The name of the response header.</param>
        /// <param name="expectedValue">The value of the response header, this could be a pattern that includes an astrix ('*').</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasResponseHeader(this ICheck<HttpResponseMessage?> check, string expectedHeader, string expectedValue)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .CheckSutAttributes(sut => sut.Headers.Select(x => new Header(x.Key, x.Value)), "headers")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailWhen(sut =>
                {
                    if (sut.Any(x => x.Name == expectedHeader))
                    {
                        var header = sut.First(x => x.Name == expectedHeader);
                        if (StringMatcher.Matches(header.Value, expectedValue))
                        {
                            return false;

                        }
                    }

                    return true;
                }, "The {0} does not contain the expected header.")
                .DefineExpectedResult(new Header(expectedHeader, expectedValue), "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has a content header with a given name.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedHeader">The name of the response header.</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContentHeader(this ICheck<HttpResponseMessage?> check, string expectedHeader)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .CheckSutAttributes(sut => sut.Content, "content")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Headers.Select(x => x.Key).ToArray(), "headers")
                .FailWhen(sut => !sut.Contains(expectedHeader), "The {0} does not contain the expected header.")
                .DefineExpectedResult(expectedHeader, "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has a content header with a given name and value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedHeader">The name of the response header.</param>
        /// <param name="expectedValue">The value of the response header, this could be a pattern that includes an astrix ('*').</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContentHeader(this ICheck<HttpResponseMessage?> check, string expectedHeader, string expectedValue)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .CheckSutAttributes(sut => sut.Content, "content")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailIfNull()
                .CheckSutAttributes(sut => sut.Headers.Select(x => new Header(x.Key, x.Value)), "headers")
                .FailWhen(sut =>
                {
                    if (sut.Any(x => x.Name == expectedHeader))
                    {
                        var header = sut.First(x => x.Name == expectedHeader);
                        if (StringMatcher.Matches(header.Value, expectedValue))
                        {
                            return false;
                        }
                    }
                    return true;
                }, "The {0} does not contain the expected header.")
                .DefineExpectedResult(new Header(expectedHeader, expectedValue), "The expected header:", "The forbidden header:")
                .OnNegate("The {0} should not contain the forbidden header.")
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has content.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContent(this ICheck<HttpResponseMessage?> check)
        {
            ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull()
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                .FailWhen(sut => sut.Content == null, "The {0} has no content, but content was expected.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .OnNegate("The {0} has content, but no content was expected.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock)
                .EndCheck();

            return ExtensibilityHelper.BuildCheckLink(check);
        }

        /// <summary>
        /// Verify that the response has content with a specific value.
        /// </summary>
        /// <param name="check">The fluent check to be extended.</param>
        /// <param name="expectedContent">The content that the response should contain, this could be a pattern that includes an astrix ('*').</param>
        /// <returns>A check link.</returns>
        public static ICheckLink<ICheck<HttpResponseMessage?>> HasContent(this ICheck<HttpResponseMessage?> check, string expectedContent)
        {
            var checkLogic = ExtensibilityHelper.BeginCheck(check)
                .SetSutName("response")
                .FailIfNull();

            if (expectedContent == null)
            {
                checkLogic
                    .Fail("The expected content should not be null, but it is.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock)
                    .CantBeNegated($"{nameof(HasContent)} with {nameof(expectedContent)} set to null")
                    .EndCheck();
            }
            else if (expectedContent.Contains("*"))
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                checkLogic.CheckSutAttributes(sut => sut.Content?.ReadAsStringAsync()?.Result, "content")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailWhen(sut => sut == null || !StringMatcher.Matches(sut, expectedContent), "The {0} does not match the expected pattern.")
                .DefineExpectedResult(expectedContent, "The expected content pattern:", "The forbidden content pattern:")
                .OnNegate("The {0} should not match the forbidden pattern.")
                .EndCheck();
            }
            else
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference. Justification = "Null reference check is performed by the FailIfNull check"
                checkLogic.CheckSutAttributes(sut => sut.Content?.ReadAsStringAsync()?.Result, "content")
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FailWhen(sut => sut == null || !StringMatcher.Matches(sut, expectedContent), "The {0} should be the expected content.")
                .DefineExpectedResult(expectedContent, "The expected content:", "The forbidden content:")
                .OnNegate("The {0} should not be the forbidden content.")
                .EndCheck();
            }

            return ExtensibilityHelper.BuildCheckLink(check);
        }
    }
}
