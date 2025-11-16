namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithFormUrlEncodedContent
{
    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_NulCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent([]));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNumberOfRequests_NulCheck_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent([], 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_NullNameValueCollection_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(null!));

        Assert.Equal("nameValueCollection", exception.ParamName);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNumberOfRequests_NullNameValueCollection_ThrowsArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(null!, 1));

        Assert.Equal("nameValueCollection", exception.ParamName);
    }

    [Fact]
    public void WithFormMatchingUrlEncodedContent_WithoutNumberOfRequests_DoesNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Content = new FormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")]);
        HttpRequestMessageAsserter sut = new([request]);

        sut.WithFormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")]);
    }

    [Fact]
    public void WithMatchingFormUrlEncodedContent_WithNumberOfRequests_DoesNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Content = new FormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")]);
        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithFormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")], 2);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
    {
        using HttpRequestMessage request = new()
        {
            Content = new FormUrlEncodedContent([])
        };
        HttpRequestMessageAsserter sut = new([request]);

        var result = sut.WithFormUrlEncodedContent([]);

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithNotMatchingContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        using HttpRequestMessage request = new()
        {
            Content = new FormUrlEncodedContent([])
        };
        HttpRequestMessageAsserter sut = new([request]);

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")]));

        Assert.Equal("Expected at least one request to be made with content 'username=alice', but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithNotMatchingContentType_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        using HttpRequestMessage request = new()
        {
            Content = new FormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")])
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("plain/text");
        HttpRequestMessageAsserter sut = new([request]);

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")]));

        Assert.Equal("Expected at least one request to be made with content 'username=alice', header 'Content-Type' and value 'application/x-www-form-urlencoded*', but no requests were made.", exception.Message);
    }
}
