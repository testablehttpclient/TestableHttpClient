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
        var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFilter(null!, "check"));
        Assert.Equal("predicate", exception.ParamName);
    }

    [Fact]
    public void WithFilter_PredicateThatDoesNotMatchAnyRequests_ThrowsAssertionException()
    {
        var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, string.Empty));
        Assert.Equal("Expected at least one request to be made, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_ThrowsAssertionException()
    {
        var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, "custom check"));
        Assert.Equal("Expected at least one request to be made with custom check, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_PredicateThatDoesMatchAnyRequests_DoesNotThrow()
    {
        var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        sut.WithFilter(x => x != null, string.Empty);
    }

#nullable disable
    [Fact]
    public void WithFilter_WithRequestExpectations_NullPredicate_ThrowsArgumentNullException()
    {
        var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());
        Assert.Throws<ArgumentNullException>("predicate", () => sut.WithFilter(null, 1, "check"));
    }
#nullable restore

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequests_ThrowsAssertionException()
    {
        var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, 1, string.Empty));
        Assert.Equal("Expected one request to be made, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesNotMatchAnyRequestsAndMessageIsGiven_ThrowsAssertionException()
    {
        var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x == null, 1, "custom check"));
        Assert.Equal("Expected one request to be made with custom check, but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFilter_WithRequestExpectation_PredicateThatDoesMatchAnyRequests_DoesNotThrow()
    {
        var sut = new HttpRequestMessageAsserter(new[] { new HttpRequestMessage(HttpMethod.Get, "https://example.com") });

        sut.WithFilter(x => x != null, 1, string.Empty);
    }

    [Fact]
    [Obsolete("Testing obsolete code")]
    public void WithFilter_WithDisposedRequestContent_DoesThrowSensibleException()
    {
        StringContent content = new("");
        content.Dispose();
        HttpRequestMessage request = new() { Content = content };
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFilter(x => x.HasContent("test"), 1, "disposed check"));
        Assert.Equal("Can't validate requests, because one or more requests have content that is already disposed.", exception.Message);
    }
}
