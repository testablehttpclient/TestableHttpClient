using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithContentHeaderName
{
    [Fact]
    public void WithContentHeader_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContentHeader_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader("someHeader", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithContentHeader_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithContentHeader_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContentHeader(headerName, 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContentHeader_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithContentHeader("Content-Type");

        sut.Received().WithFilter(Args.AnyPredicate(), null, "content header 'Content-Type'");
    }

    [Fact]
    public void WithContentHeader_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithContentHeader("Content-Type", 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "content header 'Content-Type'");
    }
}
