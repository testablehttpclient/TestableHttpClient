using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

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
                    null => "at least one request to be made",
                    0 => "no requests to be made",
                    1 => "one request to be made",
                    _ => $"{count} requests to be made"
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
        /// Asserts whether requests were made to a given URI based on a pattern.
        /// </summary>
        /// <param name="pattern">The uri pattern that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithUriPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var condition = string.Empty;
            if (pattern != "*")
            {
                condition = $"uri pattern '{pattern}'";
            }
            return With(x => x.HasMatchingUri(pattern), condition);
        }

        /// <summary>
        /// Asserts whether requests were made with a given HTTP Method.
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
        /// Asserts whether requests were made using a specific HTTP Version.
        /// </summary>
        /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithHttpVersion(Version httpVersion)
        {
            if (httpVersion == null)
            {
                throw new ArgumentNullException(nameof(httpVersion));
            }

            return With(x => x.HasHttpVersion(httpVersion), $"HTTP Version '{httpVersion}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name. Values are ignored.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithRequestHeader(string headerName)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }
            return With(x => x.HasRequestHeader(headerName), $"request header '{headerName}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name and value.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithRequestHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }
            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }
            return With(x => x.HasRequestHeader(headerName, headerValue), $"request header '{headerName}' and value '{headerValue}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name. Values are ignored.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithContentHeader(string headerName)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }
            return With(x => x.HasContentHeader(headerName), $"content header '{headerName}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name and value.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithContentHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }
            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }
            return With(x => x.HasContentHeader(headerName, headerValue), $"content header '{headerName}' and value '{headerValue}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name. Values are ignored.
        /// </summary>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithHeader(string headerName)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }
            return With(x => x.HasRequestHeader(headerName) || x.HasContentHeader(headerName), $"header '{headerName}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name and value.
        /// </summary>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }
            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }
            return With(x => x.HasRequestHeader(headerName, headerValue) || x.HasContentHeader(headerName, headerValue), $"header '{headerName}' and value '{headerValue}'");
        }

        /// <summary>
        /// Asserts whether requests were made with specific content.
        /// </summary>
        /// <param name="pattern">The expected content, supports wildcards.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithContent(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return With(x => x.HasContent(pattern), $"content '{pattern}'");
        }

        /// <summary>
        /// Asserts wheter requests are made with specific json content.
        /// </summary>
        /// <param name="jsonObject">The object representation of the expected request content.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter WithJsonContent(object? jsonObject)
        {
            var jsonString = JsonSerializer.Serialize(jsonObject);
            return With(x => x.HasContent(jsonString) && x.HasContentHeader("Content-Type", "application/json*"), $"json content '{jsonString}'");
        }

        public HttpRequestMessageAsserter WithFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            using var content = new FormUrlEncodedContent(nameValueCollection);
            var contentString = content.ReadAsStringAsync().Result;

            return With(x => x.HasContent(contentString) && x.HasContentHeader("Content-Type", "application/x-www-form-urlencoded*"), $"form url encoded content '{contentString}'");
        }

        /// <summary>
        /// Asserts that a specific amount of requests were made.
        /// </summary>
        /// <param name="count">The number of requests that are expected, should be a positive value.</param>
        /// <returns>The <seealso cref="HttpRequestMessageAsserter"/> for further assertions.</returns>
        public HttpRequestMessageAsserter Times(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Count should not be less than zero", nameof(count));
            }

            Assert(count);
            return this;
        }
    }
}
