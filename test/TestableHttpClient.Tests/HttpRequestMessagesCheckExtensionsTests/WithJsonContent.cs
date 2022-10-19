﻿using System.Text.Json;
using System.Text.RegularExpressions;

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
        var sut = new Mock<IHttpRequestMessagesCheck>();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Once());
    }

    [Fact]
    public void WithJsonContent_WithoutNumberOfRequestsWithCustomJsonSerializerOptions_DoesnotCallOptionsFromCheck()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null, new JsonSerializerOptions());

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Never());
    }

    [Fact]
    public void WithJsonContent_WithNumberOfRequests_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null, 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Once());
    }


    [Fact]
    public void WithJsonContent_WithNumberOfRequestsAndJsonSerializerOptions_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();
        sut.SetupGet(x => x.Options).Returns(new TestableHttpMessageHandlerOptions());

        sut.Object.WithJsonContent(null, new JsonSerializerOptions(), 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "json content 'null'"));
        sut.Verify(x => x.Options, Times.Never());
    }

    [Fact]
    public void WithJsonContent_RequestWithMatchingContent_ReturnsHttpRequestMessageAsserter()
    {
        var request = new HttpRequestMessage
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };
        var sut = new HttpRequestMessageAsserter(new[] { request });

        var result = sut.WithJsonContent(null);

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
    }

    [Fact]
    public void WithJsonContent_RequestWithDifferentContent_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        var request = new HttpRequestMessage
        {
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };
        var sut = new HttpRequestMessageAsserter(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithJsonContent(null));
        Assert.Equal("Expected at least one request to be made with json content 'null', but no requests were made.", exception.Message);
    }

    [Fact]
    public void WithJsonContent_RequestWithDifferentContentType_ThrowsHttpRequestMessageAssertionExceptionWithSpecificMessage()
    {
        var request = new HttpRequestMessage
        {
            Content = new StringContent("null", Encoding.UTF8)
        };
        var sut = new HttpRequestMessageAsserter(new[] { request });

        var exception = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithJsonContent(null));
        Assert.Equal("Expected at least one request to be made with json content 'null', but no requests were made.", exception.Message);
    }
}
