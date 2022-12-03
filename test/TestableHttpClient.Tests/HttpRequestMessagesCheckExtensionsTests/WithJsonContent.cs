using System.Text.Json;

using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithJsonContent
{
    [Fact]
    public void WithJsonContent_WithoutNumberOfRequests_NullChecker_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithJsonContent(null));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithJsonContent_WithNumberOfRequests_NullChecker_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithJsonContent(null, 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithJsonContent_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Once());
    }

    [Fact]
    public void WithJsonContent_WithoutNumberOfRequestsWithCustomJsonSerializerOptions_DoesnotCallOptionsFromCheck()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null, new JsonSerializerOptions());

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Never());
    }

    [Fact]
    public void WithJsonContent_WithNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null, 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Once());
    }


    [Fact]
    public void WithJsonContent_WithNumberOfRequestsAndJsonSerializerOptions_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null, new JsonSerializerOptions(), 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Never());
    }

    [Fact]
    public void WithJsonContent_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };
        HttpRequestMessageAsserter sut = new(new[] { request });

        IHttpRequestMessagesCheck result = sut.WithJsonContent(null);

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
    }

    [Fact]
    public void WithJsonContent_RequestWithDifferentContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithJsonContent(null));
        Assert.Equal("Expected at least one request to be made with json content 'null', but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithJsonContent_RequestWithDifferentContentType_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("null", Encoding.UTF8)
        };
        HttpRequestMessageAsserter sut = new(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithJsonContent(null));
        Assert.Equal("Expected at least one request to be made with json content 'null', but no requests were made.", exception.Message);
    }
}
