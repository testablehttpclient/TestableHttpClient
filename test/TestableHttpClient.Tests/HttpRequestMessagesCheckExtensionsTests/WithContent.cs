using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithContent
{
    [Fact]
    public void WithContent_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent("*"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContent_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent("*", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithContent_WithoutNumberOfRequests_NullPattern_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent(null!));

        Assert.Equal("pattern", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContent_WithNumberOfRequests_NullPattern_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent(null!, 1));

        Assert.Equal("pattern", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithContent_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithContent("some content");

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "content 'some content'");
    }

    [Fact]
    public void WithContent_WithNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithContent("some content", 1);

        sut.Received().WithFilter(Args.AnyPredicate(), (int?)1, "content 'some content'");
    }
}
