namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithHttpVersion
{
    [Fact]
    public void NullHttpVersion_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(null!));

        Assert.Equal("httpVersion", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_NullHttpVersion_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(null!, 1));

        Assert.Equal("httpVersion", exception.ParamName);
    }

    [Fact]
    public void WithMatchingVersion_DoesNotThrow()
    {
        using HttpRequestMessage request = new()
        {
            Version = HttpVersion.Version11
        };

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithHttpVersion(HttpVersion.Version11);
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithMatchingVersions_DoesNotThrow()
    {
        using HttpRequestMessage request = new()
        {
            Version = HttpVersion.Version11
        };

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithHttpVersion(HttpVersion.Version11, 2);
    }

    [Fact]
    public void WithNonMatchingVersion_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new()
        {
            Version = HttpVersion.Version10
        };

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpVersion(HttpVersion.Version11));
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithNonMatchingVersion_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new()
        {
            Version = HttpVersion.Version10
        };

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpVersion(HttpVersion.Version11, 1));
    }

    [Fact]
    public void WithNonMatchingNumberOfRequests_WithMatchingVersion_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new()
        {
            Version = HttpVersion.Version11
        };

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpVersion(HttpVersion.Version11, 2));
    }
}
