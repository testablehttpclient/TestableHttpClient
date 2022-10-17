namespace TestableHttpClient.Tests.TestabeHttpMessageHandlerAssertionExtensionsTests;

public class ShouldHaveMadeRequests
{
    [Fact]
    public void ShouldHaveMadeRequests_NullHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequests());
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void ShouldHaveMadeRequestWithNumberOfRequests_NullHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldHaveMadeRequests(1));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void ShouldHaveMadeRequests_WhenNoRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
    {
        using var sut = new TestableHttpMessageHandler();

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequests());
    }

    [Fact]
    public void ShouldHaveMadeRequestsWithNumberOfRequests_WhenNoRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
    {
        using var sut = new TestableHttpMessageHandler();

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldHaveMadeRequests(1));
    }

    [Fact]
    public async Task ShouldHaveMadeRequests_WhenRequestsWereMade_ReturnsHttpRequestMessageAsserter()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        _ = await client.GetAsync(new Uri("https://example.com/"));

        var result = sut.ShouldHaveMadeRequests();

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
        Assert.Same(sut.Options, result.Options);
    }

    [Fact]
    public async Task ShouldHaveMadeRequestsWithNumberOfRequests_WhenRequestsWereMade_ReturnsHttpRequestMessageAsserter()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        _ = await client.GetAsync(new Uri("https://example.com/"));

        var result = sut.ShouldHaveMadeRequests(1);

        Assert.NotNull(result);
        Assert.IsType<HttpRequestMessageAsserter>(result);
        Assert.Same(sut.Options, result.Options);
    }
}
