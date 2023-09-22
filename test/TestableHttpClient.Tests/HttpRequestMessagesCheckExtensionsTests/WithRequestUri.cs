using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithRequestUri
{
    [Fact]
    public void WithRequestUri_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri("*"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestUri_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri("*", 2));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestUri_NullPattern_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri(null!));

        Assert.Equal("pattern", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestUri_EmptyPattern_ThrowsArgumentException()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentException>(() => sut.WithRequestUri(string.Empty));

        Assert.Equal("pattern", exception.ParamName);
        sut.DidNotReceive().WithFilter(Args.AnyPredicate(), Arg.Any<int?>(), Arg.Any<string>());
    }

    [Fact]
    public void WithRequestUri_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        sut.WithRequestUri("https://example.com/");

        sut.Received(1).WithFilter(Args.AnyPredicate(), null, "uri pattern 'https://example.com/'");
    }

    [Fact]
    [Obsolete("Please use an overload without the 'ignoreCase', since ignoring casing is now controlled globally.", true)]
    public void WithRequestUri_WithoutNumberOfRequestsAndNotIgnoringCase_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var result = sut.WithRequestUri("https://example.com/", ignoreCase: false);

        Assert.Same(sut, result);
    }

    [Fact]
    [Obsolete("Please use an overload without the 'ignoreCase', since ignoring casing is now controlled globally.", true)]
    public void WithRequestUri_WithNumberOfRequestsAndNotIgnoringCase_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        var result = sut.WithRequestUri("https://example.com/", ignoreCase: false, 2);

        Assert.Same(sut, result);
    }
}
