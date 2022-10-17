namespace TestableHttpClient;

/// <summary>
/// This class makes it easy to create assertions on a collection of <seealso cref="HttpRequestMessage"/>s.
/// </summary>
public interface IHttpRequestMessagesCheck
{
    /// <summary>
    /// Options that could be used by several asserters.
    /// </summary>
    TestableHttpMessageHandlerOptions Options { get; }

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
}
