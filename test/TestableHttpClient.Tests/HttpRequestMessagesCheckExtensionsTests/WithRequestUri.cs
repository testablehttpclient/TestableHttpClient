using Moq;

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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithRequestUri_NullOrEmptyPattern_ThrowsArgumentNullException(string pattern)
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestUri(pattern));

        Assert.Equal("pattern", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithRequestUri_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithRequestUri("https://example.com/");

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "uri pattern 'https://example.com/'"));
    }

    [Fact]
    [Obsolete("Please use an overload without the 'ignoreCase', since ignoring casing is now controlled globally.", true)]
    public void WithRequestUri_WithoutNumberOfRequestsAndNotIgnoringCase_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Mock.Of<IHttpRequestMessagesCheck>();

        var result = sut.WithRequestUri("https://example.com/", ignoreCase: false);

        Assert.Same(sut, result);
    }

    [Fact]
    [Obsolete("Please use an overload without the 'ignoreCase', since ignoring casing is now controlled globally.", true)]
    public void WithRequestUri_WithNumberOfRequestsAndNotIgnoringCase_CallsWithCorrectly()
    {
        IHttpRequestMessagesCheck sut = Mock.Of<IHttpRequestMessagesCheck>();

        var result = sut.WithRequestUri("https://example.com/", ignoreCase: false, 2);

        Assert.Same(sut, result);
    }
}
