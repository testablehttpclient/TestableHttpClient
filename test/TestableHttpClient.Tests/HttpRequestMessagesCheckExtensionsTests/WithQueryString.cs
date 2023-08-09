using NSubstitute;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

[Obsolete("Use WithRequestUri instead, since it now properly supports QueryStrings as well", true)]
public class WithQueryString
{
    [Fact]
    public void WithQueryString_WithoutNumberOfRequests_NullCheck_ReturnsNull()
    {
        IHttpRequestMessagesCheck sut = null!;

        IHttpRequestMessagesCheck result = sut.WithQueryString("*");

        Assert.Same(sut, result);
    }

    [Fact]
    public void WithQueryString_WithNumberOfRequests_NullCheck_ReturnsNull()
    {
        IHttpRequestMessagesCheck sut = null!;

        IHttpRequestMessagesCheck result = sut.WithQueryString("*", 1);

        Assert.Same(sut, result);
    }

    [Fact]
    public void WithQueryString_WithoutNumberOfRequests_ReturnsSut()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        IHttpRequestMessagesCheck result = sut.WithQueryString("");

        Assert.Same(sut, result);
    }

    [Fact]
    public void WithQueryString_WithNumberOfRequests_ReturnsSut()
    {
        IHttpRequestMessagesCheck sut = Substitute.For<IHttpRequestMessagesCheck>();

        IHttpRequestMessagesCheck result = sut.WithQueryString("", 2);

        Assert.Same(sut, result);
    }
}
