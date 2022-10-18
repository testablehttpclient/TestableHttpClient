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
        var sut = new Mock<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestUri(pattern));

        Assert.Equal("pattern", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithRequestUri_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        sut.Object.WithRequestUri("https://example.com/");

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "uri pattern 'https://example.com/'"));
    }

    [Fact]
    public void WithRequestUri_WithoutNumberOfRequestsAndNotIgnoringCase_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        sut.Object.WithRequestUri("https://example.com/", ignoreCase: false);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "uri pattern 'https://example.com/'"));
    }

    [Fact]
    public void WithRequestUri_WithNumberOfRequestsAndNotIgnoringCase_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        sut.Object.WithRequestUri("https://example.com/", ignoreCase: false, 2);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)2, "uri pattern 'https://example.com/'"));
    }
}
