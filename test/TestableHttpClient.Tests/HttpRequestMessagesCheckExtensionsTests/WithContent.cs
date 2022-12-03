using Moq;

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
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContent(null!));

        Assert.Equal("pattern", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithContent_WithNumberOfRequests_NullPattern_ThrowsArgumentNullException()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithContent(null!, 1));

        Assert.Equal("pattern", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithContent_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithContent("some content");

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "content 'some content'"));
    }

    [Fact]
    public void WithContent_WithNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithContent("some content", 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "content 'some content'"));
    }
}
