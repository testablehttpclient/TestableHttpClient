namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithHeader")]
public class WithRequestHeaderName
{
    [Fact]
    public void WithRequestHeader_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("host"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("host", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeader_WithoutNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeader_WithoutNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!, 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty, 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithMatchingRequestHeader_WithoutNumberOfRequests_DoesNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";
        HttpRequestMessageAsserter sut = new([request]);

        sut.WithRequestHeader("Host");
    }

    [Fact]
    public void WithNotMatchingRequestHeader_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("host"));
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_DoesNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithRequestHeader("Host", 2);
    }

    [Fact]
    public void WithNotMatchingRequestHeader_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("Host", 2));
    }

    [Fact]
    public void WithMatchingRequestHeader_WithNotMatchingNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example.com";
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithRequestHeader("Host", 2));
    }
}
