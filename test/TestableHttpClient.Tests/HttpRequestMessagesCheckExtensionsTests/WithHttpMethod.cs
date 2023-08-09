using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithHttpMethod
{
    [Fact]
    public void WithHttpMethod_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(HttpMethod.Get));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpMethod_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(HttpMethod.Get, 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpMethod_WithoutNumberOfRequests_NullHttpMethod_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null!));

        Assert.Equal("httpMethod", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithHttpMethod_WithNumberOfRequests_NullHttpMethod_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(null!, 1));

        Assert.Equal("httpMethod", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithHttpMethod_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithHttpMethod(HttpMethod.Get);

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "HTTP Method 'GET'");
    }

    [Fact]
    public void WithHttpMethod_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithHttpMethod(HttpMethod.Get, 1);

        sut.Received(1).WithFilter(Args.AnyPredicate(), (int?)1, "HTTP Method 'GET'");
    }
}
