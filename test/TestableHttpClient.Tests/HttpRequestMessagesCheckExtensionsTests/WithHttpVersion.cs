using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithHttpVersion
{
#nullable disable
    [Fact]
    public void WithHttpVersion_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNulLException()
    {
        IHttpRequestMessagesCheck sut = null;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(HttpVersion.Version11));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpVersion_WithNumberOfRequests_NullCheck_ThrowsArgumentNulLException()
    {
        IHttpRequestMessagesCheck sut = null;
        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpVersion(HttpVersion.Version11, 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpVersion_WithoutNumberOfRequests_NullHttpVersion_ThrowsArgumentNullException()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHttpVersion(null));

        Assert.Equal("httpVersion", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithHttpVersion_WithNumberOfRequests_NullHttpVersion_ThrowsArgumentNullException()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHttpVersion(null, 1));

        Assert.Equal("httpVersion", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }
#nullable restore

    [Fact]
    public void WithHttpVersion_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        sut.Object.WithHttpVersion(HttpVersion.Version11);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "HTTP Version '1.1'"));
    }

    [Fact]
    public void WithHttpVersion_WithNumberOfRequests_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        sut.Object.WithHttpVersion(HttpVersion.Version11, 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "HTTP Version '1.1'"));
    }
}
