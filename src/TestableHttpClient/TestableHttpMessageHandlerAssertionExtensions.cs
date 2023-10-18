namespace TestableHttpClient;

public static class TestableHttpMessageHandlerAssertionExtensions
{
    /// <summary>
    /// Validates that requests have been made, throws an exception when no requests were made.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
    /// <exception cref="ArgumentNullException">handler is `null`</exception>
    /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
    [AssertionMethod]
    public static IHttpRequestMessagesCheck ShouldHaveMadeRequests(this TestableHttpMessageHandler handler)
    {
        Guard.ThrowIfNull(handler);

        return new HttpRequestMessageAsserter(handler.Requests, handler.Options).WithRequestUri("*");
    }

    /// <summary>
    /// Validates that requests have been made, throws an exception when no requests were made.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
    /// <exception cref="ArgumentNullException">handler is `null`</exception>
    /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
    [AssertionMethod]
    public static IHttpRequestMessagesCheck ShouldHaveMadeRequests(this TestableHttpMessageHandler handler, int expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(handler);

        return new HttpRequestMessageAsserter(handler.Requests, handler.Options).WithRequestUri("*", expectedNumberOfRequests);
    }

    /// <summary>
    /// Validates that requests to a specific uri have been made, throws an exception when no requests were made.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
    /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
    /// <exception cref="ArgumentNullException">handler is `null` or pattern is `null`</exception>
    /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
    [AssertionMethod]
    public static IHttpRequestMessagesCheck ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler handler, string pattern)
    {
        Guard.ThrowIfNull(handler);
        Guard.ThrowIfNull(pattern);

        return new HttpRequestMessageAsserter(handler.Requests, handler.Options).WithRequestUri(pattern);
    }

    /// <summary>
    /// Validates that requests to a specific uri have been made, throws an exception when no requests were made.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
    /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
    /// <param name="ignoreCase">The uri validation should ignore cases.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
    /// <exception cref="ArgumentNullException">handler is `null` or pattern is `null`</exception>
    /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
    [AssertionMethod]
    [Obsolete("Please use an overload without the 'ignoreCase', since ignoring casing is now controlled globally.")]
    public static IHttpRequestMessagesCheck ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler handler, string pattern, bool ignoreCase)
    {
        Guard.ThrowIfNull(handler);
        Guard.ThrowIfNull(pattern);

        return new HttpRequestMessageAsserter(handler.Requests, handler.Options).WithRequestUri(pattern, ignoreCase);
    }

    /// <summary>
    /// Validates that requests to a specific uri have been made, throws an exception when no requests were made.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
    /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
    /// <exception cref="ArgumentNullException">handler is `null` or pattern is `null`</exception>
    /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
    [AssertionMethod]
    public static IHttpRequestMessagesCheck ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler handler, string pattern, int expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(handler);
        Guard.ThrowIfNull(pattern);

        return new HttpRequestMessageAsserter(handler.Requests, handler.Options).WithRequestUri(pattern, expectedNumberOfRequests);
    }

    /// <summary>
    /// Validates that requests to a specific uri have been made, throws an exception when no requests were made.
    /// </summary>
    /// <param name="handler">The <see cref="TestableHttpMessageHandler"/> that should be asserted.</param>
    /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> that can be used for additional assertions.</returns>
    /// <exception cref="ArgumentNullException">handler is `null` or pattern is `null`</exception>
    /// <exception cref="HttpRequestMessageAssertionException">When no requests are made</exception>
    [AssertionMethod]
    [Obsolete("Please use an overload without the 'ignoreCase', since ignoring casing is now controlled globally.")]
    public static IHttpRequestMessagesCheck ShouldHaveMadeRequestsTo(this TestableHttpMessageHandler handler, string pattern, bool ignoreCase, int expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(handler);
        Guard.ThrowIfNull(pattern);

        return new HttpRequestMessageAsserter(handler.Requests, handler.Options).WithRequestUri(pattern, ignoreCase, expectedNumberOfRequests);
    }
}
