using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithContentHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", headerValue));

        Assert.Equal("headerValue", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithContentHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", headerValue, 1));

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
