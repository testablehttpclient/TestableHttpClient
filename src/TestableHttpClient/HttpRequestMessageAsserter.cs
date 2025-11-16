namespace TestableHttpClient;

/// <summary>
/// This class makes it easy to create assertions on a collection of <seealso cref="HttpRequestMessage"/>s.
/// </summary>
internal sealed class HttpRequestMessageAsserter : IHttpRequestMessagesCheck
{
    private readonly List<string> _expectedConditions = new();

    /// <summary>
    /// Construct a new HttpRequestMessageAsserter.
    /// </summary>
    /// <param name="httpRequestMessages">The list of requests to assert on.</param>
    /// <param name="options">Options that could be used by several assertions.</param>
    public HttpRequestMessageAsserter(IEnumerable<HttpRequestMessage> httpRequestMessages, TestableHttpMessageHandlerOptions? options = null)
    {
        Requests = httpRequestMessages ?? throw new ArgumentNullException(nameof(httpRequestMessages));
        Options = options ?? new TestableHttpMessageHandlerOptions();
    }

    /// <summary>
    /// The list of requests received from <seealso cref="TestableHttpMessageHandler"/>.
    /// </summary>
    public IEnumerable<HttpRequestMessage> Requests { get; private set; }
    /// <summary>
    /// Options that could be used by several assertions.
    /// </summary>
    public TestableHttpMessageHandlerOptions Options { get; }

    private void Assert(int? expectedCount = null)
    {
        var actualCount = Requests.Count();
        var pass = expectedCount switch
        {
            null => actualCount > 0,
            _ => actualCount == expectedCount,
        };

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
    /// <param name="condition">The name of the condition, used in the exception message.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [AssertionMethod]
    public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, string condition) => WithFilter(requestFilter, null, condition);

    /// <summary>
    /// Asserts whether requests comply with a specific filter.
    /// </summary>
    /// <param name="requestFilter">The filter to filter requests with before asserting.</param>
    /// <param name="expectedNumberOfRequests">The number of requests with this filter.</param>
    /// <param name="condition">The name of the condition, used in the exception message.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [AssertionMethod]
    public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int expectedNumberOfRequests, string condition) => WithFilter(requestFilter, (int?)expectedNumberOfRequests, condition);

    [AssertionMethod]
    public IHttpRequestMessagesCheck WithFilter(Func<HttpRequestMessage, bool> requestFilter, int? expectedNumberOfRequests, string condition)
    {
        if (!string.IsNullOrEmpty(condition))
        {
            _expectedConditions.Add(condition);
        }

        try
        {
            Requests = Requests.Where(requestFilter);
            Assert(expectedNumberOfRequests);
        }
        catch (ObjectDisposedException)
        {
            throw new HttpRequestMessageAssertionException("Can't validate requests, because one or more requests have content that is already disposed.");
        }
        return this;
    }

    /// <summary>
    /// Asserts whether requests were made to a given URI based on a pattern.
    /// </summary>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithRequestUri(string pattern) => WithRequestUri(pattern, null);

    /// <summary>
    /// Asserts whether requests were made to a given URI based on a pattern.
    /// </summary>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithRequestUri(string pattern, int expectedNumberOfRequests) => WithRequestUri(pattern, (int?)expectedNumberOfRequests);

    private IHttpRequestMessagesCheck WithRequestUri(string pattern, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNullOrEmpty(pattern);

        var condition = string.Empty;
        if (pattern != "*")
        {
            condition = $"uri pattern '{pattern}'";
        }

        UriPattern uriPattern = UriPatternParser.Parse(pattern);

        return WithFilter(x => x.RequestUri is not null && uriPattern.Matches(x.RequestUri, Options.UriPatternMatchingOptions), expectedNumberOfRequests, condition);
    }

    /// <summary>
    /// Asserts whether requests were made with a given HTTP Method.
    /// </summary>
    /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpMethod(HttpMethod httpMethod) => WithHttpMethod(httpMethod, null);

    /// <summary>
    /// Asserts whether requests were made with a given HTTP Method.
    /// </summary>
    /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpMethod(HttpMethod httpMethod, int expectedNumberOfRequests) => WithHttpMethod(httpMethod, (int?)expectedNumberOfRequests);

    private IHttpRequestMessagesCheck WithHttpMethod(HttpMethod httpMethod, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(httpMethod);

        return WithFilter(x => x.HasHttpMethod(httpMethod), expectedNumberOfRequests, $"HTTP Method '{httpMethod}'");
    }

    /// <summary>
    /// Asserts whether requests were made using a specific HTTP Version.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpVersion(Version httpVersion) => WithHttpVersion(httpVersion, null);

    /// <summary>
    /// Asserts whether requests were made using a specific HTTP Version.
    /// </summary>
    /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHttpVersion(Version httpVersion, int expectedNumberOfRequests) => WithHttpVersion(httpVersion, (int?)expectedNumberOfRequests);

    private IHttpRequestMessagesCheck WithHttpVersion(Version httpVersion, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(httpVersion);

        return WithFilter(x => x.HasHttpVersion(httpVersion), expectedNumberOfRequests, $"HTTP Version '{httpVersion}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName) => WithHeader(headerName, (int?)null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName, int expectedNumberOfRequests) => WithHeader(headerName, (int?)expectedNumberOfRequests);

    private IHttpRequestMessagesCheck WithHeader(string headerName, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNullOrEmpty(headerName);

        return WithFilter(x => x.HasHeader(headerName), expectedNumberOfRequests, $"header '{headerName}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName, string headerValue) => WithHeader(headerName, headerValue, null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithHeader(string headerName, string headerValue, int expectedNumberOfRequests) => WithHeader(headerName, headerValue, (int?)expectedNumberOfRequests);

    private IHttpRequestMessagesCheck WithHeader(string headerName, string headerValue, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(headerValue);

        return WithFilter(x => x.HasHeader(headerName, headerValue), expectedNumberOfRequests, $"header '{headerName}' and value '{headerValue}'");
    }

    /// <summary>
    /// Asserts whether requests were made with specific content.
    /// </summary>
    /// <param name="pattern">The expected content, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithContent(string pattern) => WithContent(pattern, null);

    /// <summary>
    /// Asserts whether requests were made with specific content.
    /// </summary>
    /// <param name="pattern">The expected content, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public IHttpRequestMessagesCheck WithContent(string pattern, int expectedNumberOfRequests) => WithContent(pattern, (int?)expectedNumberOfRequests);

    private IHttpRequestMessagesCheck WithContent(string pattern, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(pattern);

        return WithFilter(x => x.HasContent(pattern), expectedNumberOfRequests, $"content '{pattern}'");
    }
}
