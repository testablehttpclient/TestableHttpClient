namespace TestableHttpClient.IntegrationTests;

public class AssertingRequests
{
    [Fact]
    public void BasicAssertionsWhenNoCallsWereMade()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        testHandler.ShouldHaveMadeRequests(0);
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests());
    }

    [Fact]
    public async Task WhenAssertingCallsAreNotMade_AndCallsWereMade_AssertionExceptionIsThrow()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        testHandler.ShouldHaveMadeRequests();
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests(0));
    }

    [Fact]
    public async Task AssertingCallsAreNotMadeToSpecificUri()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        testHandler.ShouldHaveMadeRequestsTo("https://example.org", 0);
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/get", 0));
    }

    [Fact]
    public async Task AssertingCallsAreMadeToSpecificUriPattern()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        testHandler.ShouldHaveMadeRequestsTo("https://*");
        testHandler.ShouldHaveMadeRequestsTo("https://*.org/get");
        testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/*");
        testHandler.ShouldHaveMadeRequestsTo("*://httpbin.org/get");
        testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/get");
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("http://httpbin.org/get"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("https://httpbin.org/"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("https://*/post"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("https://example.org/"));
    }

    [Fact]
    public async Task AssertingCallsUsingUriPattern()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        testHandler.ShouldHaveMadeRequests().WithRequestUri("https://*");
        testHandler.ShouldHaveMadeRequests().WithRequestUri("https://*.org/get");
        testHandler.ShouldHaveMadeRequests().WithRequestUri("https://httpbin.org/*");
        testHandler.ShouldHaveMadeRequests().WithRequestUri("*://httpbin.org/get");
        testHandler.ShouldHaveMadeRequests().WithRequestUri("https://httpbin.org/get");
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("http://httpbin.org/get"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("https://httpbin.org/"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("https://*/post"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("https://example.org/"));
    }

    [Fact]
    public async Task ChainUriPatternAssertions()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        testHandler.ShouldHaveMadeRequestsTo("https://*")
                   .WithRequestUri("*://httpbin.org/*")
                   .WithRequestUri("*/get");
    }

    [Fact]
    public async Task AssertingCallWithQueryParameters()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get?email=admin@example.com");

        testHandler.ShouldHaveMadeRequests().WithRequestUri("?email=admin@example.com");
        testHandler.ShouldHaveMadeRequests().WithRequestUri("?email=*");
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestUri("?email=admin%40example.com"));
    }

    [Fact]
    public async Task AssertingHttpMethods()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");
        using StringContent content = new("");
        _ = await client.PostAsync("https://httpbin.org/post", content);

        testHandler.ShouldHaveMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Get);
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Post));
        testHandler.ShouldHaveMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Post);
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Get));
    }

    [Fact]
    public async Task AssertingRequestHeaders()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);
        client.DefaultRequestHeaders.Add("api-version", "1.0");
        _ = await client.GetAsync("https://httpbin.org/get");

        testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version");
        testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "1.0");
        testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "1*");

        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestHeader("my-version"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "1"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithRequestHeader("api-version", "2*"));
    }

    [Fact]
    public async Task AssertingContentHeaders()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        using StringContent content = new("", Encoding.UTF8, "application/json");
        _ = await client.PostAsync("https://httpbin.org/post", content);

        testHandler.ShouldHaveMadeRequests().WithContentHeader("content-type");
        testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type");
        testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "application/json; charset=utf-8");
        testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "application/json*");
        testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "*charset=utf-8");

        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Disposition"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "application/json"));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContentHeader("Content-Type", "*=utf-16"));
    }

    [Fact]
    public async Task AssertingContent()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        using StringContent content = new("my special content");
        _ = await client.PostAsync("https://httpbin.org/post", content);

#if NETFRAMEWORK
        // On .NET Framework the HttpClient disposes the content automatically. So we can't perform the same test.
        testHandler.ShouldHaveMadeRequests();
#else
        testHandler.ShouldHaveMadeRequests().WithContent("my special content");
        testHandler.ShouldHaveMadeRequests().WithContent("my*content");
        testHandler.ShouldHaveMadeRequests().WithContent("*");

        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContent(""));
        Assert.Throws<HttpRequestMessageAssertionException>(() => testHandler.ShouldHaveMadeRequests().WithContent("my"));
#endif
    }

    [Fact]
    public async Task AssertJsonContent()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        using StringContent content = new("{}", Encoding.UTF8, "application/json");
        _ = await client.PostAsync("https://httpbin.org/post", content);

#if NETFRAMEWORK
        // On .NET Framework the HttpClient disposes the content automatically. So we can't perform the same test.
        testHandler.ShouldHaveMadeRequests();
#else
        testHandler.ShouldHaveMadeRequests().WithJsonContent(new { });
#endif
    }

    [Fact]
    public async Task CustomAssertions()
    {
        using TestableHttpMessageHandler testHandler = new();
        using HttpClient client = new(testHandler);

        using StringContent content = new("", Encoding.UTF8, "application/json");
        _ = await client.PostAsync("https://httpbin.org/post", content);

        testHandler.ShouldHaveMadeRequests().WithFilter(x => x.Content is not null && x.Content.Headers.ContentType?.MediaType == "application/json", "");
    }
}
