namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithHeader")]
public class WithRequestHeaderNameAndValue
{
    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("host", "example.com"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("host", "example.com", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!, "example.com"));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty, "example.com"));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!, "example.com", 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty, "example.com", 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("host", null!));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader("host", string.Empty));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("host", null!, 1));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader("host", string.Empty, 1));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("*.com")]
    [InlineData("example*")]
    [InlineData("example.com")]
    public void WithMatchingRequestHeaderNameAndValue_WithoutNumberOfRequests_DoesNotThrow(string headerValue)
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithRequestHeader("host", headerValue);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("*.com")]
    [InlineData("example*")]
    [InlineData("example.com")]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_DoesNotThrow(string headerValue)
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithRequestHeader("host", headerValue, 2);
    }

    [Fact]
    public void WithNotMatchingRequestHeaderNameAndMatchingValue_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("Via", "example.com"));
    }

    [Fact]
    public void WithNotMatchingRequestHeaderNameAndMatchingValue_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("Via", "example.com", 2));
    }

    [Fact]
    public void WithMatchingRequestHeaderNameAndNotMatchingValue_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("Host", "text/yaml*"));
    }

    [Fact]
    public void WithMatchingRequestHeaderNameAndNotMatchingValue_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("Host", "text/yaml*", 2));
    }

    [Fact]
    public void WithMatchingRequestHeaderNameAndMatchingValue_WithNotMatchingNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("Host", "example.com", 2));
    }
}
