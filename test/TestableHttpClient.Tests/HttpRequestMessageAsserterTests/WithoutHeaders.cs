namespace TestableHttpClient.Tests.HttpRequestMessageAsserterTests;

public sealed class WithoutHeaders
{
    [Fact]
    public void WithoutHeadersInRequest_ShouldNotThrow()
    {
        using HttpRequestMessage request = new();

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithoutHeaders();
    }

    [Fact]
    public void WithExpectedNumberOfRequests_WithoutHeadersInRequest_ShouldNotThrow()
    {
        using HttpRequestMessage request = new();

        HttpRequestMessageAsserter sut = new([request]);

        sut.WithoutHeaders(1);
    }

    [Fact]
    public void WithHeadersInRequest_ShouldThrow()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithoutHeaders());
    }

    [Fact]
    public void WithExpectedNumberOfRequests_WithHeadersInRequest_ShouldNotThrow()
    {
        using HttpRequestMessage request = new();
        request.Headers.Host = "example";

        HttpRequestMessageAsserter sut = new([request]);

        Assert.Throws<HttpRequestMessageAssertionException>(() => sut.WithoutHeaders(1));
    }
}
