using System;

namespace TestableHttpClient
{
    public static class TestableHttpMessageHandlerAssertionExtensions
    {
        /// <summary>
        /// Validates that requests have been made, throws an exception when no requests were made.
        /// </summary>
        /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
        /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
        /// <exception cref="ArgumentNullException">handler is `null`</exception>
        /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
        public static IHttpRequestMessagesCheck ShouldHaveMadeRequests(this TestableHttpMessageHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return new HttpRequestMessageAsserter(handler.Requests).WithRequestUri("*");
        }

        /// <summary>
        /// Validates that requests to a specific uri have been made, throws an exception when no requests were made.
        /// </summary>
        /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
        /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
        /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
        /// <exception cref="ArgumentNullException">handler is `null` or pattern is `null`</exception>
        /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
        public static IHttpRequestMessagesCheck ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler handler, string pattern)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return new HttpRequestMessageAsserter(handler.Requests).WithRequestUri(pattern);
        }

        /// <summary>
        /// Validates that no requests have been made, throws an exception when requests were made.
        /// </summary>
        /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
        /// <exception cref="ArgumentNullException">handler is `null`</exception>
        /// <exception cref="HttpRequestMessageAssertionException">When requests are made</exception>
        public static void ShouldNotHaveMadeRequests(this TestableHttpMessageHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _ = new HttpRequestMessageAsserter(handler.Requests, true).WithRequestUri("*");
        }

        /// <summary>
        /// Validates that no requests to a specific uri have been made, throws an exception when requests were made.
        /// </summary>
        /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
        /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
        /// <exception cref="ArgumentNullException">handler is `null` or pattern is `null`</exception>
        /// <exception cref="HttpRequestMessageAssertionException">When requests are made</exception>
        public static void ShouldNotHaveMadeRequestsTo(this TestableHttpMessageHandler handler, string pattern)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            _ = new HttpRequestMessageAsserter(handler.Requests, true).WithRequestUri(pattern);
        }
    }
}
