using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithFormUrlEncodedContent
{
    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_NulCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string?, string?>>()));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNumberOfRequests_NulCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string?, string?>>(), 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_NullNameValueCollection_ThrowsArgumentNullException()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithFormUrlEncodedContent(null!));

        Assert.Equal("nameValueCollection", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNumberOfRequests_NullNameValueCollection_ThrowsArgumentNullException()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithFormUrlEncodedContent(null!, 1));

        Assert.Equal("nameValueCollection", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithFormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("username", "alice") });

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "form url encoded content 'username=alice'"));
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithFormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("username", "alice") }, 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "form url encoded content 'username=alice'"));
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
    {
        using HttpRequestMessage request = new()
        {
            Content = new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string?, string?>>())
        };
        HttpRequestMessageAsserter sut = new(new[] { request });

        var result = sut.WithFormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string?, string?>>());

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithNotMatchingContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        using HttpRequestMessage request = new()
        {
            Content = new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string?, string?>>())
        };
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("username", "alice") }));

        Assert.Equal("Expected at least one request to be made with form url encoded content 'username=alice', but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_RequestWithNotMatchingContentType_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        using HttpRequestMessage request = new()
        {
            Content = new FormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("username", "alice") })
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("plain/text");
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithFormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("username", "alice") }));

        Assert.Equal("Expected at least one request to be made with form url encoded content 'username=alice', but no requests were made.", exception.Message);
    }
}
