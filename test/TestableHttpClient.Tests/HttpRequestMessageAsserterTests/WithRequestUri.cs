namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithRequestUri
{
    [Fact]
    public void NullRequestUri_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri(null!));

        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void EmptyRequestUri_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestUri(string.Empty));

        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void WithMatchingRequestUri_DoesNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        sut.WithRequestUri("https://example.com/");
    }

    [Fact]
    public void WithNonMatchingRequestUri_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://no-op.com/"));

    }


    [Fact]
    public void WithNonMatchinRequestUri_WithCustomOptions_ThrowsHttpRequestMessageAssertionException()
    {
        TestableHttpMessageHandlerOptions options = new();
        options.UriPatternMatchingOptions.HostCaseInsensitive = false;

        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");

        HttpRequestMessageAsserter sut = new([request], options);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://EXAMPLE.com/"));
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithMatchingRequestUri_DoesNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithRequestUri("https://example.com/", 2);
    }

    [Fact]
    public void WithNonMatchingNumberOfRequest_WithMatchingRequestUris_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://example.com/", 2));
    }

    [Fact]
    public void WithNonMatchingNumberOfRequest_WithNonMatchingRequestUris_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://no-op.com/", 2));
    }
}
