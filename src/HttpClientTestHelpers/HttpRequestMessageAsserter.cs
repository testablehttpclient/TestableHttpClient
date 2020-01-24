using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HttpClientTestHelpers
{
    /// <summary>
    /// This class makes it easy to create assertions on a collection of <seealso cref="HttpRequestMessage"/>s.
    /// </summary>
    public class HttpRequestMessageAsserter
    {
        private readonly List<string> _expectedConditions = new List<string>();
        private readonly bool _negate = false;

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

        private void Assert(int? count = null)
        {
            var actualCount = Requests.Count();
            var pass = count switch
            {
                null => Requests.Any(),
                _ => actualCount == count,
            };

            if (_negate)
            {
                if (!count.HasValue)
                {
                    count = 0;
                }
                pass = !pass;
            }

            if (!pass)
            {
                var expected = count switch
                {
                    0 => "no requests to be made",
                    _ => "at least one request to be made",
                };
                var actual = actualCount switch
                {
                    0 => "no requests were made",
                    1 => "one request was made",
                    _ => $"{actualCount} requests were made",
                };

                if (_expectedConditions.Any())
                {
                    var conditions = string.Join(", ", _expectedConditions);
                    expected += $" with {conditions}";
                }

                var message = $"Expected {expected}, but {actual}.";
                throw new HttpRequestMessageAssertionException(message);
            }
        }

        /// <summary>
        /// Asserts whether requests comply with a specific filter.
        /// </summary>
        /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
        /// <param name="condition">The name of the conditon, used in the exception message.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter With(Func<HttpRequestMessage, bool> requestFilter, string condition)
        {
            if (!string.IsNullOrEmpty(condition))
            {
                _expectedConditions.Add(condition);
            }

            Requests = Requests.Where(requestFilter);
            Assert();
            return this;
        }

        /// <summary>
        /// Asserts wheter requests were made to a given URI based on a pattern.
        /// </summary>
        /// <param name="pattern">The uri pattern that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithUriPattern(string pattern)
        {
            if(string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            string condition = string.Empty;
            if (pattern != "*")
            {
                condition = $"uri pattern '{pattern}'";
            }
            return With(x => x.HasMatchingUri(pattern), condition);
        }

        /// <summary>
        /// Asserts wheter requests were made with a given HTTP Method.
        /// </summary>
        /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithHttpMethod(HttpMethod httpMethod)
        {
            if (httpMethod == null)
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            return With(x => x.HasHttpMethod(httpMethod), $"HTTP Method '{httpMethod}'");
        }

        /// <summary>
        /// Asserts wheter requests were made using a specific HTTP Version.
        /// </summary>
        /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithHttpVersion(Version httpVersion)
        {
            if(httpVersion == null)
            {
                throw new ArgumentNullException(nameof(httpVersion));
            }

            return With(x => x.HasHttpVersion(httpVersion), $"HTTP Version '{httpVersion}'");
        }
    }
}
