using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithHeaderName
{
    [Fact]
    public void WithHeader_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHeader_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader("someHeader", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithHeader_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(headerName));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithHeader_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHeader(headerName, 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithHeader_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithHeader("Content-Type");

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "header 'Content-Type'");
    }

    [Fact]
    public void WithHeader_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithHeader("Content-Type", 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "header 'Content-Type'");
    }
}
