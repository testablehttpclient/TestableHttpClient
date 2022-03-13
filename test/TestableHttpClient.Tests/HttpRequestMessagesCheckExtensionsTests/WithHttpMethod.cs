using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithHttpMethod
{
#nullable disable
    [Fact]
    public void WithHttpMethod_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(HttpMethod.Get));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpMethod_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithHttpMethod(HttpMethod.Get, 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithHttpMethod_WithoutNumberOfRequests_NullHttpMethod_ThrowsArgumentNullException()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHttpMethod(null));

        Assert.Equal("httpMethod", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithHttpMethod_WithNumberOfRequests_NullHttpMethod_ThrowsArgumentNullException()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithHttpMethod(null, 1));

        Assert.Equal("httpMethod", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }
#nullable restore

    [Fact]
    public void WithHttpMethod_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        sut.Object.WithHttpMethod(HttpMethod.Get);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "HTTP Method 'GET'"));
    }

    [Fact]
    public void WithHttpMethod_WithNumberOfRequests_CallsWithCorrectly()
    {
        var sut = new Mock<IHttpRequestMessagesCheck>();

        sut.Object.WithHttpMethod(HttpMethod.Get, 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "HTTP Method 'GET'"));
    }
}
