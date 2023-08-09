using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithHttpVersion
{
    [Fact]
    public void WithHttpVersion_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNulLException()
    {
        IHttpRequestMessagesCheck sut = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(HttpVersion.Version11));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpVersion_WithNumberOfRequests_NullCheck_ThrowsArgumentNulLException()
    {
        IHttpRequestMessagesCheck sut = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(HttpVersion.Version11, 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpVersion_WithoutNumberOfRequests_NullHttpVersion_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(null!));

        Assert.Equal("httpVersion", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithHttpVersion_WithNumberOfRequests_NullHttpVersion_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(null!, 1));

        Assert.Equal("httpVersion", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithHttpVersion_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithHttpVersion(HttpVersion.Version11);

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "HTTP Version '1.1'");
    }

    [Fact]
    public void WithHttpVersion_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithHttpVersion(HttpVersion.Version11, 1);

        sut.Received().WithFilter(Args.AnyPredicate(), (int?)1, "HTTP Version '1.1'");
    }
}
