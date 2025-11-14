using NSubstitute;

using TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithRequestUri
{
    [Fact]
    public void NullPattern_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri(null!));

        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void EmptyPattern_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestUri(string.Empty));

        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void WithPatternMatchingARequest_DoesNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        sut.WithRequestUri("https://example.com/");
    }

    [Fact]
    public void WithPatternNotMatchingAnyRequest_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://no-op.com/"));
    }


    [Fact]
    public void WithPatternNotMatchingAnyRequestWithCustomOptions_ThrowsHttpRequestMessageAssertionException()
    {
        TestableHttpMessageHandlerOptions options = new();
        options.UriPatternMatchingOptions.HostCaseInsensitive = false;

        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");

        HttpRequestMessageAsserter sut = new([request], options);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://EXAMPLE.com/"));
    }

    [Fact]
    public void WithPatternMatchingASingleRequest_DoesNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        sut.WithRequestUri("https://example.com/", 1);
    }

    [Fact]
    public void WithPatternMatchingMultipleRequestsButOnlyOneReceived_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestUri("https://no-op.com/", 2));
    }
}
