namespace TestableHttpClient.Tests;

public partial class HttpRequestMessageExtensionsTests
{
    [Fact]
    public void HasHttpMethod_WithHttpMethod_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod(HttpMethod.Get));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasHttpMethod_WithString_NullRequest_ThrowsArgumentNullException()
    {
        HttpRequestMessage sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod("GET"));
        Assert.Equal("httpRequestMessage", exception.ParamName);
    }

    [Fact]
    public void HasHttpMethod_WithHttpMethod_NullHttpMethod_ThrowsArgumentNullException()
    {
        using HttpRequestMessage sut = new() { Method = HttpMethod.Get };

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod((HttpMethod)null!));
        Assert.Equal("httpMethod", exception.ParamName);
    }

    [Fact]
    public void HasHttpMethod_WithString_NullHttpMethod_ThrowsArgumentNullException()
    {
        using HttpRequestMessage sut = new() { Method = HttpMethod.Get };

        var exception = Assert.Throws<ArgumentNullException>(() => sut.HasHttpMethod((string)null!));
        Assert.Equal("httpMethod", exception.ParamName);
    }

    [Fact]
    public void HasHttpMethod_WithString_EmptyHttpMethod_ThrowsArgumentException()
    {
        using HttpRequestMessage sut = new() { Method = HttpMethod.Get };

        var exception = Assert.Throws<ArgumentException>(() => sut.HasHttpMethod(string.Empty));
        Assert.Equal("httpMethod", exception.ParamName);
    }

    [Fact]
    public void HasHttpMethod_WithHttpMethod_CorrectHttpMethod_ReturnsTrue()
    {
        using HttpRequestMessage sut = new() { Method = HttpMethod.Get };

        Assert.True(sut.HasHttpMethod(HttpMethod.Get));
    }

    [Theory]
    [InlineData("GET")]
    [InlineData("Get")]
    [InlineData("get")]
    public void HasHttpMethod_WithString_CorrectHttpMethod_ReturnsTrue(string httpMethod)
    {
        using HttpRequestMessage sut = new() { Method = HttpMethod.Get };

        Assert.True(sut.HasHttpMethod(httpMethod));
    }

    [Fact]
    public void HasHttpMethod_WithHttpMethod_IncorrectHttpMethod_ReturnsFalse()
    {
        using HttpRequestMessage sut = new() { Method = HttpMethod.Get };

        Assert.False(sut.HasHttpMethod(HttpMethod.Put));
    }

    [Fact]
    public void HasHttpMethod_WithString_IncorrectHttpMethod_ReturnsFalse()
    {
        using HttpRequestMessage sut = new() { Method = HttpMethod.Get };

        Assert.False(sut.HasHttpMethod("DELETE"));
    }
}
