using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithHeader")]
public class WithContentHeaderNameAndValue
{
    [Fact]
    public void WihtContentHeaderNameAndValue_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", "someValue"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WihtContentHeaderNameAndValue_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", "someValue", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(null!, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader(string.Empty, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullHeaderName_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(null!, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_EmptyHeaderName_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader(string.Empty, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", null!));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader("someHeader", string.Empty));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullValue_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", null!, 1));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_EmptyValue_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithContentHeader("someHeader", string.Empty, 1));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithContentHeader("someHeader", "someValue");

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "content header 'someHeader' and value 'someValue'");
    }

    [Fact]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithContentHeader("someHeader", "someValue", 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "content header 'someHeader' and value 'someValue'");
    }
}
