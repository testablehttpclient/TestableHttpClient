using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithHeader")]
public class WithRequestHeaderName
{
    [Fact]
    public void WithRequestHeader_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeader_WithoutNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeader_WithoutNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!, 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty, 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeader_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithRequestHeader("api-version");

        sut.Received().WithFilter(Args.AnyPredicate(), null, "request header 'api-version'");
    }

    [Fact]
    public void WithRequestHeader_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithRequestHeader("api-version", 1);

        sut.Received().WithFilter(Args.AnyPredicate(), (int?)1, "request header 'api-version'");
    }
}
