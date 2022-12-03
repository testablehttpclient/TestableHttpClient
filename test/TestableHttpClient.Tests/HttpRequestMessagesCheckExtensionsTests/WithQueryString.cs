using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithRequestUri instead, since it now properly supports QueryStrings as well")]
public class WithQueryString
{
    [Fact]
    public void WithQueryString_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithQueryString("*"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithQueryString_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithQueryString("*", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithQueryString_WithoutNumberOfRequests_NullPattern_ThrowsArgumentNullException()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithQueryString(null!));

        Assert.Equal("pattern", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithQueryString_WithNumberOfRequests_NullPattern_ThrowsArgumentNullException()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithQueryString(null!, 1));

        Assert.Equal("pattern", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithQueryString_WithoutNumberOfRequests_WithoutPattern_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithQueryString("");

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "no querystring"));
    }

    [Fact]
    public void WithQueryString_WithNumberOfRequests_WithoutPattern_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithQueryString("", 2);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)2, "no querystring"));
    }

    [Fact]
    public void WithQueryString_WithoutNumberOfRequests_WithStarPattern_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithQueryString("*");

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "any querystring"));
    }

    [Fact]
    public void WithQueryString_WithNumberOfRequests_WithStarPattern_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithQueryString("*", 2);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)2, "any querystring"));
    }

    [Fact]
    public void WithQueryString_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithQueryString("email=test@example.com");

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "querystring pattern 'email=test@example.com'"));
    }

    [Fact]
    public void WithQueryString_WithNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithQueryString("email=test@example.com", 2);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)2, "querystring pattern 'email=test@example.com'"));
    }
}
