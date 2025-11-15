namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithHeaderName
{
    [Fact]
    public void NullHeaderName_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(null!));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void EmptyHeaderName_ShouldThrowArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithHeader(string.Empty));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_NullHeaderName_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(null!, 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_EmptyHeaderName_ShouldThrowArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithHeader(string.Empty, 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Theory]
    [InlineData("Content-Type")]
    [InlineData("content-type")]
    [InlineData("CONTENT-type")]
    public void WithMatchingHeaderName_DoesNotThrow(string headerName)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithHeader(headerName);
    }

    [Fact]
    public void WithNotMatchingHeaderName_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "localhost";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type"));
    }

    [Theory]
    [InlineData("Content-Type")]
    [InlineData("content-type")]
    [InlineData("CONTENT-type")]
    public void WithMatchingNumberOfRequests_WithMatchingHeaderName_DoesNotThrow(string headerName)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithHeader(headerName, 2);
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithNotMatchingHeaderName_DoesNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "localhost";

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type", 2));
    }

    [Theory]
    [InlineData("Content-Type")]
    [InlineData("content-type")]
    [InlineData("CONTENT-type")]
    public void WithNotMatchingNumberOfRequests_WithMatchingHeaderName_ThrowsHttpRequestMessageAssertionException(string headerName)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader(headerName, 1));
    }
}
