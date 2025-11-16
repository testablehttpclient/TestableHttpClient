namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithHttpMethod
{
    [Fact]
    public void WithNullHttpMethod_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = new HttpRequestMessageAsserter(Array.Empty<HttpRequestMessage>());
        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null!));
        // Assert
        Assert.Equal("httpMethod", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_WithNullHttpMethod_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = new HttpRequestMessageAsserter(Array.Empty<HttpRequestMessage>());
        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null!, 1));
        // Assert
        Assert.Equal("httpMethod", exception.ParamName);
    }

    [Fact]
    public void WithMatchingHttpMethod_ShouldNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithHttpMethod(HttpMethod.Get);
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithMatchingHttpMethod_ShouldNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithHttpMethod(HttpMethod.Get, 2);
    }

    [Fact]
    public void WithNonMatchingNumberOfRequests_WithMatchingHttpMethod_ShouldNotThrow()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Get, 1));
    }

    [Fact]
    public void WithNonMatchingHttpMethod_ShouldThrowHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Post));
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithNonMatchingHttpMethod_ShouldThrowHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, "https://example.com");
        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHttpMethod(HttpMethod.Post, 2));
    }
}
