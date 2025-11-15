using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithHeader")]
public class WithRequestHeaderNameAndValue
{
    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", "someValue"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", "someValue", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(null!, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader(string.Empty, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", null!));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader("someHeader", string.Empty));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", null!, 1));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestHeader("someHeader", string.Empty, 1));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithRequestHeader("someHeader", "someValue");

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "request header 'someHeader' and value 'someValue'");
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithRequestHeader("someHeader", "someValue", 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "request header 'someHeader' and value 'someValue'");
    }
}
