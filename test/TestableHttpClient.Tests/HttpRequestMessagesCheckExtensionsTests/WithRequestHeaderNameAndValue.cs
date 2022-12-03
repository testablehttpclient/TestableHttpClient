using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public class WithRequestHeaderNameAndValue
{
    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", "someValue"));

        Assert.Equal("check", exception.ParamName);
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullCheck_ThrowsArgumentNullException()
    {
        IHttpRequestMessagesCheck sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithRequestHeader("someHeader", "someValue", 1));

        Assert.Equal("check", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestHeader(headerName, "someValue"));

        Assert.Equal("headerName", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyHeaderName_ThrowsArgumentNullException(string headerName)
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestHeader(headerName, "someValue", 1));

        Assert.Equal("headerName", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestHeader("someHeader", headerValue));

        Assert.Equal("headerValue", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_NullOrEmptyValue_ThrowsArgumentNullException(string headerValue)
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.Object.WithRequestHeader("someHeader", headerValue, 1));

        Assert.Equal("headerValue", exception.ParamName);
        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), It.IsAny<int?>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithoutNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithRequestHeader("someHeader", "someValue");

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), null, "request header 'someHeader' and value 'someValue'"));
    }

    [Fact]
    public void WithRequestHeaderNameAndValue_WithNumberOfRequests_CallsWithCorrectly()
    {
        Mock<IHttpRequestMessagesCheck> sut = new();

        sut.Object.WithRequestHeader("someHeader", "someValue", 1);

        sut.Verify(x => x.WithFilter(Its.AnyPredicate(), (int?)1, "request header 'someHeader' and value 'someValue'"));
    }
}
