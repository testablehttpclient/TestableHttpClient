using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using NFluent;
using NFluent.Extensibility;
using NFluent.Kernel;

using TestableHttpClient.Utils;

namespace TestableHttpClient.NFluent
{
    /// <summary>
    /// Class that implement NFluent checks on a collection of HttpRequestMessages.
    /// </summary>
    internal class FluentHttpRequestMessagesChecks : FluentSut<IEnumerable<HttpRequestMessage>>, IHttpRequestMessagesCheck
    {
        private IEnumerable<HttpRequestMessage> requests;
        private readonly List<string> requestConditions = new List<string>();

        public FluentHttpRequestMessagesChecks(IEnumerable<HttpRequestMessage> httpRequestMessages)
            : base(httpRequestMessages, Check.Reporter, false)
        {
            requests = httpRequestMessages ?? throw new ArgumentNullException(nameof(httpRequestMessages));
        }

        [Obsolete("With is a language keyword and should be avoided, use WithFilter instead.", true)]
        public IHttpRequestMessagesCheck With(Func<HttpRequestMessage, bool> requestFilter, string condition) => WithFilter(requestFilter, condition);

        public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, string condition) => WithFilter(requestFilter, null, condition);

        public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int expectedNumberOfRequests, string condition) => WithFilter(requestFilter, (int?)expectedNumberOfRequests, condition);

        public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int? expectedNumberOfRequests, string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                requestConditions.Add(condition);
            }

            var checkLogic = ExtensibilityHelper.BeginCheck(this)
                .CantBeNegated(nameof(WithFilter))
                .FailWhen(_ => requestFilter == null, "The request filter should not be null.", MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock)
                .Analyze((sut, _) => requests = requests.Where(requestFilter));

            AnalyzeNumberOfRequests(checkLogic, expectedNumberOfRequests);
            return this;
        }

        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        public IHttpRequestMessagesCheck Times(int count)
        {
            var checkLogic = ExtensibilityHelper.BeginCheck(this)
                .CantBeNegated(nameof(Times))
                .SetSutName("number of requests")
                .DefineExpectedResult(count, null, null)
                .FailWhen(_ => count < 0, "The {1} should not be below zero.", MessageOption.NoCheckedBlock);
            AnalyzeNumberOfRequests(checkLogic, count);
            return this;
        }

        private void AnalyzeNumberOfRequests(ICheckLogic<IEnumerable<HttpRequestMessage>> checkLogic, int? expectedCount)
        {
            checkLogic.Analyze((sut, check) =>
            {
                var actualCount = requests.Count();
                var pass = expectedCount switch
                {
                    null => actualCount > 0,
                    _ => actualCount == expectedCount
                };

                var message = MessageBuilder.BuildMessage(expectedCount, actualCount, requestConditions);
                if (!pass)
                {
                    check.Fail(message, MessageOption.NoCheckedBlock | MessageOption.NoExpectedBlock);
                }
            })
            .EndCheck();
        }
    }
}
