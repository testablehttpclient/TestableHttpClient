namespace TestableHttpClient.Tests.TestabeHttpMessageHandlerAssertionExtensionsTests;

[Obsolete("Tested method is obsolete")]
public class ShouldNotHaveMadeRequests
{
#nullable disable
    [Fact]
    public void ShouldNotHaveMadeRequests_NullHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.ShouldNotHaveMadeRequests());
        Assert.Equal("handler", exception.ParamName);
    }
#nullable restore

    [Fact]
    public void ShouldNotHaveMadeRequests_WhenNoRequestsWereMade_DoesNotThrowExceptions()
    {
        using var sut = new TestableHttpMessageHandler();

        sut.ShouldNotHaveMadeRequests();
    }

    [Fact]
    public async Task ShouldNotHaveMadeRequests_WhenASingleRequestWasMade_ThrowsHttpRequestMessageAssertionException()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        _ = await client.GetAsync(new Uri("https://example.com/"));

        var result = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldNotHaveMadeRequests());
        Assert.Equal("Expected no requests to be made, but one request was made.", result.Message);
    }

    [Fact]
    public async Task ShouldNotHaveMadeRequests_WhenMultipleRequestsWereMade_ThrowsHttpRequestMessageAssertionException()
    {
        using var sut = new TestableHttpMessageHandler();
        using var client = new HttpClient(sut);

        _ = await client.GetAsync(new Uri("https://example.com/"));
        _ = await client.GetAsync(new Uri("https://example.com/"));

        var result = Assert.Throws<HttpRequestMessageAssertionException>(() => sut.ShouldNotHaveMadeRequests());
        Assert.Equal("Expected no requests to be made, but 2 requests were made.", result.Message);
    }
}
