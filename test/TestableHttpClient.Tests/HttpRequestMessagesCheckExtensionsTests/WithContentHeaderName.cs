namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithHeader")]
public class WithContentHeaderName
{
    [Fact]
    public void WithContentHeader_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContentHeader_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContentHeader_WithoutNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(null!));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithContentHeader_WithoutNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader(string.Empty));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithContentHeader_WithNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(null!, 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithContentHeader_WithNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader(string.Empty, 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithMatchingContentHeader_WithoutNumberOfRequests_DoesNotThrow()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");
        HttpRequestMessageAsserter sut = new([request]);

        sut.WithContentHeader("Content-Type");
    }

    [Fact]
    public void WithNoContent_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type"));
    }

    [Fact]
    public void WithNotMatchingContentHeader_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");
        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Disposition"));
    }

    [Fact]
    public void WithContentHeader_WithNumberOfRequests_DoesNotThrow()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");
        HttpRequestMessageAsserter sut = new([request]);

        sut.WithContentHeader("Content-Type", 1);
    }

    [Fact]
    public void WithNoContent_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", 2));
    }

    [Fact]
    public void WithNotMatchingContentHeader_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");
        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Disposition", 2));
    }
}
