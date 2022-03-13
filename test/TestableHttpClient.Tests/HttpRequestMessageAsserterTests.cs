namespace TestableHttpClient.Tests;

public class HttpRequestMessageAsserterTests
{
#nullable disable
    [Fact]
    public void Constructor_NullRequestList_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new HttpRequestMessageAsserter(null));
    }

    [Fact]
    public void WithFilter_NullPredicate_ThrowsArgumentNullException()
    {
        var sut = new HttpRequestMessageAsserter(Enumerable.Empty<HttpRequestMessage>());
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFilter(null, "check"));
        Assert.Equal("predicate", exception.ParamName);
    }
#nullable restore

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
}
