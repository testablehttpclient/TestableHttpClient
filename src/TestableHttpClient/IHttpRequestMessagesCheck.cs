using System;
using System.Net.Http;

namespace TestableHttpClient
{
    /// <summary>
    /// This class makes it easy to create assertions on a collection of <seealso cref="HttpRequestMessage"/>s.
    /// </summary>
    public interface IHttpRequestMessagesCheck
    {
        /// <summary>
        /// Asserts whether requests comply with a specific filter.
        /// </summary>
        /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
        /// <param name="condition">The name of the conditon, used in the exception message.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        IHttpRequestMessagesCheck With(Func<HttpRequestMessage, bool> requestFilter, string condition);

        /// <summary>
        /// Asserts that a specific amount of requests were made.
        /// </summary>
        /// <param name="count">The number of requests that are expected, should be a positive value.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        IHttpRequestMessagesCheck Times(int count);
    }
}
