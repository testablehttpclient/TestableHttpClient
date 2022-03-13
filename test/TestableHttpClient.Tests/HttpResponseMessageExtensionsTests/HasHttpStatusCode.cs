namespace TestableHttpClient.Tests;

public partial class HttpResponseMessageExtensionsTests
{
#nullable disable
    [Fact]
    public void HasHttpStatusCode_WithHttpStatusCode_NullHttpResponseMessage_ThrowsArgumentNullException()
    {
        HttpResponseMessage sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpStatusCode(HttpStatusCode.OK));
        Assert.Equal("httpResponseMessage", exception.ParamName);
    }
#nullable restore

    [Fact]
    public void HasHttpStatusCode_WithHttpStatusCode_CorrectHttpStatusCode_ReturnsTrue()
    {
        using var sut = new HttpResponseMessage(HttpStatusCode.OK);

        Assert.True(sut.HasHttpStatusCode(HttpStatusCode.OK));
    }

    [Fact]
    public void HasHttpStatusCode_WithHttpStatusCode_IncorrectHttpStatusCode_ReturnsFalse()
    {
        using var sut = new HttpResponseMessage(HttpStatusCode.BadRequest);

        Assert.False(sut.HasHttpStatusCode(HttpStatusCode.OK));
    }
}
