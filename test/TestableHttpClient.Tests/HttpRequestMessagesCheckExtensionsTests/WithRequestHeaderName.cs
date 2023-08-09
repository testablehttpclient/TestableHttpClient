using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithRequestHeader_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(headerName));

        Assert.Equal("headerName", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithRequestHeader_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader(headerName, 1));

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
