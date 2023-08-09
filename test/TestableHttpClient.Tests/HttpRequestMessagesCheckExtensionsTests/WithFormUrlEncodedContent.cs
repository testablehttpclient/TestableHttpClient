using NSubstitute;

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
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(null!));

        Assert.Equal("nameValueCollection", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNumberOfRequests_NullNameValueCollection_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(null!, 1));

        Assert.Equal("nameValueCollection", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithFormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("username", "alice") });

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "form url encoded content 'username=alice'");
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithFormUrlEncodedContent(new[] { new KeyValuePair<string?, string?>("username", "alice") }, 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "form url encoded content 'username=alice'");
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
