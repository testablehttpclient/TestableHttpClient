namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithContent
{
    [Fact]
    public void NullContentPattern_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent(null!));

        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void WithNumberOfRequests_NullContentPattern_ShouldThrowArgumentNullException()
    {
        HttpRequestMessageAsserter sut = new([]);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithContent(null!, 1));

        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void EmptyContentPattern_NullContentInRequest_ShouldThrowHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent(""));
    }

    [Fact]
    public void WithNumberOfRequests_EmptyContentPattern_NullContentInRequest_ShouldThrowHttpRequestMessageAssertionException()
    {
        using HttpRequestMessage request = new();

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent("", 1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void MatchingContent_DoesNotThrow(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent(content)
        };

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithContent(content);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void WithMatchingNumberOfRequests_MatchingContent_DoesNotThrow(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent(content)
        };

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithContent(content, 2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void WithNotMatchingNumberOfRequests_MatchingContent_ThrowsHttpRequestMessageAssertionException(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent(content)
        };

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent(content, 1));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void NotMatchingContent_ThrowsHttpRequestMessageAssertionException(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("Example content")
        };

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent(content));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Some text")]
    [InlineData("{\"key\":\"value\"}")]
    public void WithMatchingNumberOfRequests_NotMatchingContent_ThrowsHttpRequestMessageAssertionException(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("Example content")
        };

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent(content, 2));
    }

    [Theory]
    [InlineData("*")]
    [InlineData("username=*&password=*")]
    [InlineData("*admin*")]
    public void MatchingPattern_DoesNotThrow(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("username=admin&password=admin")
        };

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithContent(content);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("username=*&password=*")]
    [InlineData("*admin*")]
    public void WithMatchingNumberOfRequests_MatchingPattern_DoesNotThrow(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("username=admin&password=admin")
        };

        HttpRequestMessageAsserter sut = new([request, request]);

        sut.WithContent(content, 2);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("username=*&password=*")]
    [InlineData("*admin*")]
    public void WithNotMatchingNumberOfRequests_MatchingPattern_ThrowsHttpRequestMessageAssertionException(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("username=admin&password=admin")
        };

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent(content, 1));
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("*test*")]
    public void NotMatchingPattern_ThrowsHttpRequestMessageAssertionException(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("username=admin&password=admin")
        };

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent(content));
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("*test*")]
    public void WithMatchingNumberOfRequests_NotMatchingPattern_ThrowsHttpRequestMessageAssertionException(string content)
    {
        using HttpRequestMessage request = new()
        {
            Content = new StringContent("username=admin&password=admin")
        };

        HttpRequestMessageAsserter sut = new([request, request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithContent(content, 2));
    }
}
