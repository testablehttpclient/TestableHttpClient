namespace TestableHttpClient;

/// <summary>
/// This class makes it easy to create assertions on a collection of <seealso cref="HttpRequestMessage"/>s.
/// </summary>
internal sealed class HttpRequestMessageAsserter : IHttpRequestMessagesCheck
{
    private readonly List<string> _expectedConditions = new();
    private Request expectedRequest;

    /// <summary>
    /// Construct a new HttpRequestMessageAsserter.
    /// </summary>
    /// <param name="httpRequestMessages">The list of requests to assert on.</param>
    /// <param name="options">Options that could be used by several assertions.</param>
    public HttpRequestMessageAsserter(IEnumerable<HttpRequestMessage> httpRequestMessages, TestableHttpMessageHandlerOptions? options = null)
    {
        Requests = httpRequestMessages ?? throw new ArgumentNullException(nameof(httpRequestMessages));
        Options = options ?? new TestableHttpMessageHandlerOptions();
        expectedRequest = new Request(Options.UriPatternMatchingOptions);
    }

    /// <summary>
    /// The list of requests received from <seealso cref="TestableHttpMessageHandler"/>.
    /// </summary>
    public IEnumerable<HttpRequestMessage> Requests { get; private set; }
    /// <summary>
    /// Options that could be used by several assertions.
    /// </summary>
    public TestableHttpMessageHandlerOptions Options { get; }

    private HttpRequestMessageAsserter Assert(int? expectedCount = null, string condition = "")
    {
        if (!string.IsNullOrEmpty(condition))
        {
            _expectedConditions.Add(condition);
        }
        Assert(expectedCount);
        return this;
    }

    private void Assert(int? expectedCount = null)
    {
        int actualCount;
        try
        {
            actualCount = Requests.Count(expectedRequest.Equals);
        }
        catch (ObjectDisposedException)
        {
            throw new HttpRequestMessageAssertionException("Can't validate requests, because one or more requests have content that is already disposed.");
        }

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

        Requests = Requests.Where(requestFilter);
        Assert(expectedNumberOfRequests);

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

    private HttpRequestMessageAsserter WithRequestUri(string pattern, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNullOrEmpty(pattern);

        var condition = string.Empty;
        if (pattern != "*")
        {
            condition = $"uri pattern '{pattern}'";
        }

        UriPattern uriPattern = UriPatternParser.Parse(pattern);
        expectedRequest = expectedRequest with { RequestUri = uriPattern };

        return Assert(expectedNumberOfRequests, condition);
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

    private HttpRequestMessageAsserter WithHttpMethod(HttpMethod httpMethod, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(httpMethod);
        expectedRequest = expectedRequest with { HttpMethod = httpMethod };
        return Assert(expectedNumberOfRequests, $"HTTP Method '{httpMethod}'");
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

    private HttpRequestMessageAsserter WithHttpVersion(Version httpVersion, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(httpVersion);

        expectedRequest = expectedRequest with { HttpVersion = httpVersion };

        return Assert(expectedNumberOfRequests, $"HTTP Version '{httpVersion}'");
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

    private HttpRequestMessageAsserter WithHeader(string headerName, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNullOrEmpty(headerName);

        expectedRequest = expectedRequest.AddHeader(headerName);
        return Assert(expectedNumberOfRequests, $"header '{headerName}'");
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

    private HttpRequestMessageAsserter WithHeader(string headerName, string headerValue, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(headerValue);

        expectedRequest = expectedRequest.AddHeader(headerName, headerValue);

        return Assert(expectedNumberOfRequests, $"header '{headerName}' and value '{headerValue}'");
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

    private HttpRequestMessageAsserter WithContent(string pattern, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(pattern);

        expectedRequest = expectedRequest with { Content = pattern };
        return Assert(expectedNumberOfRequests, $"content '{pattern}'");
    }
}
