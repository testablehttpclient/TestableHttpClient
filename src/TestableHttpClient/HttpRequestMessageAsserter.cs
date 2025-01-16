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
}
