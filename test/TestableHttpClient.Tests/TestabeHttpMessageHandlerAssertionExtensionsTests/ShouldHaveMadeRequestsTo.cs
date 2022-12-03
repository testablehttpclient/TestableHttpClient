namespace TestableHttpClient.Tests.TestabeHttpMessageHandlerAssertionExtensionsTests;

public class ShouldHaveMadeRequestsTo
{
    [Fact]
    public void ShouldHaveMadeRequestsTo_NullHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/"));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void ShouldHaveMadeRequestsToWithNumberOfRequests_NullHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/", 1));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void ShouldHaveMadeRequestsTo_NullPattern_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequestsTo(null!));
        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void ShouldHaveMadeRequestsToWithNumberOfRequests_NullPattern_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequestsTo(null!, 1));
        Assert.Equal("pattern", exception.ParamName);
    }

    [Fact]
    public void ShouldHaveMadeRequestsTo_WhenNoRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
    {
        using TestableHttpMessageHandler sut = new();

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/"));
    }

    [Fact]
    public void ShouldHaveMadeRequestsToWithNumberOfRequests_WhenNoRequestsWereMadeAndOneIsExpected_ThrowsHttpRequestMessageAssertionException()
    {
        using TestableHttpMessageHandler sut = new();

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/", 1));
    }

    [Fact]
    public void ShouldHaveMadeRequestsToWithNumberOfRequests_WhenNoRequestsWereMadeAndZeroRequestsAreExpected_ReturnsHttpRequestMessageAsserter()
    {
        using TestableHttpMessageHandler sut = new();

        var result = sut.ShouldHaveMadeRequestsTo("https://example.com/", 0);

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
        Assert.Same(sut.Options, result.Options);
    }

    [Fact]
    public async Task ShouldHaveMadeRequestsTo_WhenMatchingRequestsWithSameCaseWereMade_ReturnsHttpRequestMessageAsserter()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);

        _ = await client.GetAsync(new Uri("https://example.com/"));

        var result = sut.ShouldHaveMadeRequestsTo("https://example.com/");

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
        Assert.Same(sut.Options, result.Options);
    }

    [Fact]
    public async Task ShouldHaveMadeRequestsToWithNumberOfRequests_WhenMatchingRequestsWithSameCaseWereMade_ReturnsHttpRequestMessageAsserter()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);

        _ = await client.GetAsync(new Uri("https://example.com/"));

        var result = sut.ShouldHaveMadeRequestsTo("https://example.com/", 1);

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
        Assert.Same(sut.Options, result.Options);
    }

    [Fact]
    public async Task ShouldHaveMadeRequestsTo_WhenMatchingRequestsWithDifferentCaseWereMade_ReturnsHttpRequestMessageAsserter()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);

        _ = await client.GetAsync(new Uri("https://Example.com/Test"));

        var result = sut.ShouldHaveMadeRequestsTo("https://example.com/test");

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
        Assert.Same(sut.Options, result.Options);
    }

    [Fact]
    public async Task ShouldHaveMadeRequestsTo_WhenMatchingRequestsWithDifferentCaseWereMadeAndNotIgnoringCase_ThrowsException()
    {
        using TestableHttpMessageHandler sut = new();
        sut.Options.UriPatternMatchingOptions.PathCaseInsensitive = false;
        using HttpClient client = new(sut);

        _ = await client.GetAsync(new Uri("https://Example.com/Test"));

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/test"));
    }

    [Fact]
    public async Task ShouldHaveMadeRequestsToWithNumberOfRequests_WhenMatchingRequestsWithDifferentCaseWereMade_ReturnsHttpRequestMessageAsserter()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = new(sut);

        _ = await client.GetAsync(new Uri("https://Example.com/Test"));

        var result = sut.ShouldHaveMadeRequestsTo("https://example.com/test", 1);

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
        Assert.Same(sut.Options, result.Options);
    }

    [Fact]
    public async Task ShouldHaveMadeRequestsToWithNumberOfRequests_WhenMatchingRequestsWithDifferentCaseWereMadeAndNotIgnoringCase_ShouldThrowException()
    {
        using TestableHttpMessageHandler sut = new();
        sut.Options.UriPatternMatchingOptions.PathCaseInsensitive = false;
        using HttpClient client = new(sut);

        _ = await client.GetAsync(new Uri("https://Example.com/Test"));

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequestsTo("https://example.com/test", 1));
    }
}
