namespace TestableHttpClient.NFluent;

/// <summary>
/// A set of NFluent checks to check <see cref="TestableHttpMessageHandler"/>.
/// </summary>
public static class TestableHttpClientChecks
{
    /// <summary>
    /// Verify that at least one request is made.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> which can be used to make more specific checks.</returns>
    public static IHttpRequestMessagesCheck HasMadeRequests(this ICheck<TestableHttpMessageHandler?> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailIfNull()
            .CantBeNegated(nameof(HasMadeRequests))
            .EndCheck();

        return check.HasMadeRequestsTo("*");
    }

    /// <summary>
    /// Verify that at least one request is made.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> which can be used to make more specific checks.</returns>
    public static IHttpRequestMessagesCheck HasMadeRequests(this ICheck<TestableHttpMessageHandler?> check, int expectedNumberOfRequests)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailIfNull()
            .CantBeNegated(nameof(HasMadeRequests))
            .EndCheck();

        return check.HasMadeRequestsTo("*", expectedNumberOfRequests);
    }

    /// <summary>
    /// Verify that at least one request is made to a specific url.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> which can be used to make more specific checks.</returns>
    public static IHttpRequestMessagesCheck HasMadeRequestsTo(this ICheck<TestableHttpMessageHandler?> check, string pattern)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailIfNull()
            .CantBeNegated(nameof(HasMadeRequestsTo))
            .EndCheck();

        var requests = Enumerable.Empty<HttpRequestMessage>();
        if (check is FluentSut<TestableHttpMessageHandler> checker && checker.Value != null && checker.Value.Requests != null)
        {
            requests = checker.Value.Requests;
        }

        return new FluentHttpRequestMessagesChecks(requests).WithRequestUri(pattern);
    }

    /// <summary>
    /// Verify that at least one request is made to a specific url.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>An <see cref="IHttpRequestMessagesCheck"/> which can be used to make more specific checks.</returns>
    public static IHttpRequestMessagesCheck HasMadeRequestsTo(this ICheck<TestableHttpMessageHandler?> check, string pattern, int expectedNumberOfRequests)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailIfNull()
            .CantBeNegated(nameof(HasMadeRequestsTo))
            .EndCheck();

        var requests = Enumerable.Empty<HttpRequestMessage>();
        if (check is FluentSut<TestableHttpMessageHandler> checker && checker.Value != null && checker.Value.Requests != null)
        {
            requests = checker.Value.Requests;
        }

        return new FluentHttpRequestMessagesChecks(requests).WithRequestUri(pattern, expectedNumberOfRequests);
    }
}
