using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HttpClientTestHelpers
{
    public class HttpRequestMessageAsserter
    {
        private readonly List<string> _expectedConditions = new List<string>();
        private readonly bool _negate = false;

        public HttpRequestMessageAsserter(IEnumerable<HttpRequestMessage> httpRequestMessages)
        {
            Requests = httpRequestMessages ?? throw new ArgumentNullException(nameof(httpRequestMessages));
        }

        public HttpRequestMessageAsserter(IEnumerable<HttpRequestMessage> httpRequestMessages, bool negate)
        {
            Requests = httpRequestMessages ?? throw new ArgumentNullException(nameof(httpRequestMessages));
            _negate = negate;
        }

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

        public HttpRequestMessageAsserter WithUriPattern(string pattern)
        {
            if (pattern != "*")
            {
                _expectedConditions.Add($"uri pattern '{pattern}'");
            }

            Requests = Requests.Where(x => x.HasMatchingUri(pattern));
            Assert();
            return this;
        }
    }
}
