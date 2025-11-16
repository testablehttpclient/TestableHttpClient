namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithHeader")]
public class WithContentHeaderNameAndValue
{
    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", "someValue"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", "someValue", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(null!, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader(string.Empty, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(null!, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader(string.Empty, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", null!));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader("someHeader", string.Empty));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", null!, 1));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader("someHeader", string.Empty, 1));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("application/*")]
    [InlineData("application/json*")]
    [InlineData("application/json; charset=utf-8")]
    public void WithMatchingContentHeaderNameAndValue_WithoutNumberOfRequests_DoesNotThrow(string headerValue)
    {
        using HttpRequestMessage request = new();
        request.Content = new StringContent("", Encoding.UTF8, "application/json");

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithContentHeader("Content-Type", headerValue);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("application/*")]
    [InlineData("application/json*")]
    [InlineData("application/json; charset=utf-8")]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_DoesNotThrow(string headerValue)
    {
        using HttpRequestMessage request = new();
        request.Content = new StringContent("", Encoding.UTF8, "application/json");

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithContentHeader("Content-Type", headerValue, 2);
    }

    [Fact]
    public void WithNoContent_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Content = null;

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json*"));
    }

    [Fact]
    public void WithNoContent_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Content = null;

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json*", 2));
    }

    [Fact]
    public void WithNotMatchingContentHeaderNameAndMatchingValue_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Content = new StringContent("", Encoding.UTF8, "application/json");

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Host", "application/json*"));
    }

    [Fact]
    public void WithNotMatchingContentHeaderNameAndMatchingValue_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Content = new StringContent("", Encoding.UTF8, "application/json");

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Host", "application/json*", 2));
    }

    [Fact]
    public void WithMatchingContentHeaderNameAndNotMatchingValue_WithoutNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Content = new StringContent("", Encoding.UTF8, "application/json");

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "text/yaml*"));
    }

    [Fact]
    public void WithMatchingContentHeaderNameAndNotMatchingValue_WithNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Content = new StringContent("", Encoding.UTF8, "application/json");

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "text/yaml*", 2));
    }

    [Fact]
    public void WithMatchingContentHeaderNameAndMatchingValue_WithNotMatchingNumberOfRequests_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Content = new StringContent("", Encoding.UTF8, "application/json");

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContentHeader("Content-Type", "application/json*", 2));
    }
}
