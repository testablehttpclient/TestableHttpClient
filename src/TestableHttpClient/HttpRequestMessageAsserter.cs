using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using TestableHttpClient.Utils;

namespace TestableHttpClient
{
    /// <summary>
    /// This class makes it easy to create assertions on a collection of <seealso cref="HttpRequestMessage"/>s.
    /// </summary>
    internal class HttpRequestMessageAsserter : IHttpRequestMessagesCheck
    {
        private readonly List<string> _expectedConditions = new List<string>();
        private readonly bool _negate;

        /// <summary>
        /// Construct a new HttpRequestMessageAsserter.
        /// </summary>
        /// <param name="httpRequestMessages">The list of requests to assert on.</param>
        public HttpRequestMessageAsserter(IEnumerable<HttpRequestMessage> httpRequestMessages)
            : this(httpRequestMessages, false)
        {
        }

        /// <summary>
        /// Construct a new HttpRequestMessageAsserter.
        /// </summary>
        /// <param name="httpRequestMessages">The list of requests to assert on.</param>
        /// <param name="negate">Whether or not all assertions should be negated.</param>
        public HttpRequestMessageAsserter(IEnumerable<HttpRequestMessage> httpRequestMessages, bool negate)
        {
            Requests = httpRequestMessages ?? throw new ArgumentNullException(nameof(httpRequestMessages));
            _negate = negate;
        }

        /// <summary>
        /// The list of requests received from <seealso cref="TestableHttpMessageHandler"/>.
        /// </summary>
        public IEnumerable<HttpRequestMessage> Requests { get; private set; }

        private void Assert(int? expectedCount = null)
        {
            var actualCount = Requests.Count();
            var pass = expectedCount switch
            {
                null => actualCount > 0,
                _ => actualCount == expectedCount,
            };

            if (_negate)
            {
                if (!expectedCount.HasValue)
                {
                    expectedCount = 0;
                }
                pass = !pass;
            }

            if (!pass)
            {
                var message = MessageBuilder.BuildMessage(expectedCount, actualCount, _expectedConditions);
                throw new HttpRequestMessageAssertionException(message);
            }
        }

        /// <summary>
        /// Asserts whether requests comply with a specific filter.
        /// </summary>
        /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
        /// <param name="condition">The name of the conditon, used in the exception message.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, string condition) => WithFilter(requestFilter, null, condition);

        /// <summary>
        /// Asserts whether requests comply with a specific filter.
        /// </summary>
        /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
        /// <param name="condition">The name of the conditon, used in the exception message.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int expectedNumberOfRequests, string condition) => WithFilter(requestFilter, (int?)expectedNumberOfRequests, condition);

        public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int? expectedNumberOfRequests, string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                _expectedConditions.Add(condition);
            }

            Requests = Requests.Where(requestFilter);
            Assert(expectedNumberOfRequests);
            return this;
        }
    }
}
