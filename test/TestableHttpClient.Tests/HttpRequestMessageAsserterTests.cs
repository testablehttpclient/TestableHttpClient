namespace TestableHttpClient.Tests;

public class HttpRequestMessageAsserterTests
{
    [Fact]
    public void Constructor_NullRequestList_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new HttpRequestMessageAsserter(null!));
    }

    [Fact]
    public void Constructor_NullOptions_SetsDefault()
    {
        HttpRequestMessageAsserter sut = new(Array.Empty<HttpRequestMessage>(), null);
        Assert.NotNull(sut.Options);
    }

    [Fact]
    public void Constructor_NotNullOptions_SetsOptions()
    {
        TestableHttpMessageHandlerOptions options = new();
        HttpRequestMessageAsserter sut = new(Array.Empty<HttpRequestMessage>(), options);
        Assert.Same(options, sut.Options);
    }

    [Fact]
    public void WithFilter_NullPredicate_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new(Enumerable.Empty<HttpRequestMessage>());
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFilter(null!, "check"));
        Assert.Equal("predicate", exception.ParamName);
    }

    [Fact]
    public void WithFilter_PredicateThatDoesNotMatchAnyRequests_ThrowsAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, string.Empty));
        Assert.Equal("Expected at least one request to be made, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_ThrowsAssertionException()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, "custom check"));
        Assert.Equal("Expected at least one request to be made with custom check, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_PredicateThatDoesMatchAnyRequests_DoesNotThrow()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new(new[] { request });

        sut.WithFilter(x => x != null, string.Empty);
    }

    [Fact]
    public void WithFilter_WithRequestExpectations_NullPredicate_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new(Enumerable.Empty<HttpRequestMessage>());
        Assert.Throws<ArgumentNullException>("predicate", () => sut.WithFilter(null!, 1, "check"));
    }

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequests_ThrowsAssertionException()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, 1, string.Empty));
        Assert.Equal("Expected one request to be made, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_ThrowsAssertionException()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, 1, "custom check"));
        Assert.Equal("Expected one request to be made with custom check, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesMatchAnyRequests_DoesNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new(new[] { request });

        sut.WithFilter(x => x != null, 1, string.Empty);
    }

    [Fact]
    public void WithFilter_WithDisposedRequestContent_DoesThrowSensibleException()
    {
        StringContent content = new("");
        content.Dispose();
        using HttpRequestMessage request = new() { Content = content };
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent("test", 1));
        Assert.Equal("Can't validate requests, because one or more requests have content that is already disposed.", exception.Message);
    }
}
