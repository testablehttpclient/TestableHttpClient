namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithHeaderNameAndValue
{
    [Fact]
    public void NullHeaderNameAndFilledValue_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(null!, "value"));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void EmptyHeaderNameAndFilledValue_ShouldThrowArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithHeader(string.Empty, "value"));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void FilledHeaderNameAndNullValue_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("header", null!));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void FilledHeaderNameAndEmptyValue_ShouldThrowArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithHeader("header", string.Empty));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_NullHeaderNameAndFilledValue_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(null!, "value", 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_EmptyHeaderNameAndFilledValue_ShouldThrowArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithHeader(string.Empty, "value", 1));

        Assert.Equal("headerName", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_FilledHeaderNameAndNullValue_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("header", null!, 1));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_FilledHeaderNameAndEmptyValue_ShouldThrowArgumentException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentException>(() => sut.WithHeader("header", string.Empty, 1));

        Assert.Equal("headerValue", exception.ParamName);
    }

    [Theory]
    [InlineData("Content-Type")]
    [InlineData("content-type")]
    [InlineData("CONTENT-type")]
    public void WithMatchingHeaderNameAndMatchingValue_DoesNotThrow(string headerName)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithHeader(headerName, "application/json");
    }

    [Fact]
    public void WithNotMatchingHeaderNameAndMatchingValue_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "localhost";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type", "localhost"));
    }

    [Fact]
    public void WithMatchingHeaderNameAnNotdMatchingValue_ThrowsHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "localhost";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("host", "application/json"));
    }

    [Theory]
    [InlineData("Content-Type")]
    [InlineData("content-type")]
    [InlineData("CONTENT-type")]
    public void WithMatchingNumberOfRequests_WithMatchingHeaderNameAndMatchingValue_DoesNotThrow(string headerName)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithHeader(headerName, "application/json", 2);
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithNotMatchingHeaderNameAndMatchingValue_DoesNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "localhost";

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("Content-Type", "localhost", 2));
    }

    [Fact]
    public void WithMatchingNumberOfRequests_WithMatchingHeaderNameAndNotMatchingValue_DoesNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "localhost";

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader("host", "application/json", 2));
    }

    [Theory]
    [InlineData("Content-Type")]
    [InlineData("content-type")]
    [InlineData("CONTENT-type")]
    public void WithNotMatchingNumberOfRequests_WithMatchingHeaderNameAndMatchingValue_ThrowsHttpRequestMessageAssertionException(string headerName)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("")
        };
        request.Content.Headers.ContentType = new("application/json");

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithHeader(headerName, "application/json", 1));
    }
}
