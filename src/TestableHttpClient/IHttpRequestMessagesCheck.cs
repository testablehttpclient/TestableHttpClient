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
        [Obsolete("With is a language keyword and should be avoided, use WithFilter instead.", true)]
#pragma warning disable CA1716 // Identifiers should not match keywords
        IHttpRequestMessagesCheck With(Func<HttpRequestMessage, bool> requestFilter, string condition);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Asserts whether requests comply with a specific filter.
        /// </summary>
        /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
        /// <param name="condition">The name of the conditon, used in the exception message.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, string condition);

        /// <summary>
        /// Asserts whether requests comply with a specific filter.
        /// </summary>
        /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
        /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
        /// <param name="condition">The name of the conditon, used in the exception message.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int expectedNumberOfRequests, string condition);

        /// <summary>
        /// Asserts whether requests comply with a specific filter.
        /// </summary>
        /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
        /// <param name="expectedNumberOfRequests">The expected number of requests, when null is passed "at least one" is presumed.</param>
        /// <param name="condition">The name of the conditon, used in the exception message.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int? expectedNumberOfRequests, string condition);

        /// <summary>
        /// Asserts that a specific amount of requests were made.
        /// </summary>
        /// <param name="count">The number of requests that are expected, should be a positive value.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        [Obsolete("Times as a seperate check is no longer supported, use the With overload with expectdNumberOfRequests.")]
        IHttpRequestMessagesCheck Times(int count);
    }
}
