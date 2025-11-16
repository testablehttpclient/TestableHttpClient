namespace TestableHttpClient;

/// <summary>
/// This class makes it easy to create assertions on a collection of <seealso cref="HttpRequestMessage"/>s.
/// </summary>
public interface IHttpRequestMessagesCheck
{
    /// <summary>
    /// Options that could be used by several asserters.
    /// </summary>
    public TestableHttpMessageHandlerOptions Options { get; }

    /// <summary>
    /// Asserts whether requests comply with a specific filter.
    /// </summary>
    /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
    /// <param name="condition">The name of the condition, used in the exception message.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("WithFilter will be made internal, since it should no longer be necesary to use.")]
    public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, string condition);

    /// <summary>
    /// Asserts whether requests comply with a specific filter.
    /// </summary>
    /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <param name="condition">The name of the condition, used in the exception message.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("WithFilter will be made internal, since it should no longer be necesary to use.")]
    public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int expectedNumberOfRequests, string condition);

    /// <summary>
    /// Asserts whether requests comply with a specific filter.
    /// </summary>
    /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests, when null is passed "at least one" is presumed.</param>
    /// <param name="condition">The name of the condition, used in the exception message.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("WithFilter will be made internal, since it should no longer be necesary to use.")]
    public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int? expectedNumberOfRequests, string condition);

    /// <summary>
    /// Asserts whether requests were made with a given HTTP Method.
    /// </summary>
    /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpMethod(HttpMethod httpMethod);
    /// <summary>
    /// Asserts whether requests were made with a given HTTP Method.
    /// </summary>
    /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpMethod(HttpMethod httpMethod, int expectedNumberOfRequests);

    /// <summary>
    /// Asserts whether requests were made to a given URI based on a pattern.
    /// </summary>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithRequestUri(string pattern);

    /// <summary>
    /// Asserts whether requests were made to a given URI based on a pattern.
    /// </summary>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithRequestUri(string pattern, int expectedNumberOfRequests);

    /// <summary>
    /// Asserts whether requests were made using a specific HTTP Version.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpVersion(Version httpVersion);

    /// <summary>
    /// Asserts whether requests were made using a specific HTTP Version.
    /// </summary>
    /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpVersion(Version httpVersion, int expectedNumberOfRequests);

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName);

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName, int expectedNumberOfRequests);

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName, string headerValue);

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName, string headerValue, int expectedNumberOfRequests);

    /// <summary>
    /// Asserts whether requests were made with specific content.
    /// </summary>
    /// <param name="pattern">The expected content, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithContent(string pattern);

    /// <summary>
    /// Asserts whether requests were made with specific content.
    /// </summary>
    /// <param name="pattern">The expected content, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithContent(string pattern, int expectedNumberOfRequests);
}
