using System.Text.Json;

using NSubstitute;

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
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();
        sut.Options.Returns(new TestableHttpMessageHandlerOptions());

        sut.WithJsonContent(null);

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "json content 'null'");
        _ = sut.Received(1).Options;
    }

    [Fact]
    public void WithJsonContent_WithoutNumberOfRequestsWithCustomJsonSerializerOptions_DoesNotCallOptionsFromCheck()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();
        sut.Options.Returns(new TestableHttpMessageHandlerOptions());

        sut.WithJsonContent(null, new JsonSerializerOptions());

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "json content 'null'");
        _ = sut.DidNotReceive().Options;
    }

    [Fact]
    public void WithJsonContent_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();
        sut.Options.Returns(new TestableHttpMessageHandlerOptions());

        sut.WithJsonContent(null, 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "json content 'null'");
        _ = sut.Received(1).Options;
    }


    [Fact]
    public void WithJsonContent_WithNumberOfRequestsAndJsonSerializerOptions_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();
        sut.Options.Returns(new TestableHttpMessageHandlerOptions());

        sut.WithJsonContent(null, new JsonSerializerOptions(), 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "json content 'null'");
        _ = sut.DidNotReceive().Options;
    }

    [Fact]
    public void WithJsonContent_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };
        HttpRequestMessageAsserter sut = new([request]);

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
        HttpRequestMessageAsserter sut = new([request]);

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
        HttpRequestMessageAsserter sut = new([request]);

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithJsonContent(null));
        Assert.Equal("Expected at least one request to be made with json content 'null', but no requests were made.", exception.Message);
    }
}
