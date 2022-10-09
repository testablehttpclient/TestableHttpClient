namespace TestableHttpClient.IntegrationTests;

public class AssertingRequestsWithNFluent
{
    [Fact]
    public void BasicAssertionsWhenNoCallsWereMade()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests()).IsAFailingCheck();
    }

    [Fact]
    public async Task WhenAssertingCallsAreNotMade_AndCallsWereMade_AssertionExceptionIsThrow()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        Check.That(testHandler).HasMadeRequests();
    }


    [Fact]
    public async Task AssertingCallsAreNotMadeToSpecificUri()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/get");
    }

    [Fact]
    public async Task AssertingCallsAreMadeToSpecificUriPattern()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        Check.That(testHandler).HasMadeRequestsTo("https://*");
        Check.That(testHandler).HasMadeRequestsTo("https://*.org/get");
        Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/*");
        Check.That(testHandler).HasMadeRequestsTo("*://httpbin.org/get");
        Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/get");
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("http://httpbin.org/get")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("https://httpbin.org/")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("https://*/post")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("https://example.org/")).IsAFailingCheck();
    }

    [Fact]
    public async Task AssertingCallsUsingUriPattern()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        Check.That(testHandler).HasMadeRequests().WithRequestUri("https://*");
        Check.That(testHandler).HasMadeRequests().WithRequestUri("https://*.org/get");
        Check.That(testHandler).HasMadeRequests().WithRequestUri("https://httpbin.org/*");
        Check.That(testHandler).HasMadeRequests().WithRequestUri("*://httpbin.org/get");
        Check.That(testHandler).HasMadeRequests().WithRequestUri("https://httpbin.org/get");
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("http://httpbin.org/get")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("https://httpbin.org/")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("https://*/post")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestUri("https://example.org/")).IsAFailingCheck();
    }

    [Fact]
    public async Task ChainUriPatternAssertions()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");

        Check.That(testHandler).HasMadeRequestsTo("https://*")
                   .WithRequestUri("*://httpbin.org/*")
                   .WithRequestUri("*/get");
    }

    [Fact]
    public async Task AssertingCallsUsingQueryStringPattern()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get?email=admin@example.com");

        Check.That(testHandler).HasMadeRequests().WithQueryString("email=admin@example.com");
        Check.That(testHandler).HasMadeRequests().WithQueryString("email=*");
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithQueryString("")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithQueryString("email=admin%40example.com")).IsAFailingCheck();
    }

    [Fact]
    public async Task AssertingHttpMethods()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        _ = await client.GetAsync("https://httpbin.org/get");
        using var content = new StringContent("");
        _ = await client.PostAsync("https://httpbin.org/post", content);

        Check.That(testHandler).HasMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Get);
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("*/get").WithHttpMethod(HttpMethod.Post)).IsAFailingCheck();
        Check.That(testHandler).HasMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Post);
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequestsTo("*/post").WithHttpMethod(HttpMethod.Get)).IsAFailingCheck();
    }

    [Fact]
    public async Task AssertingRequestHeaders()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);
        client.DefaultRequestHeaders.Add("api-version", "1.0");
        _ = await client.GetAsync("https://httpbin.org/get");

        Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version");
        Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "1.0");
        Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "1*");

        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestHeader("my-version")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "1")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithRequestHeader("api-version", "2*")).IsAFailingCheck();
    }

    [Fact]
    public async Task AssertingContentHeaders()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        using var content = new StringContent("", Encoding.UTF8, "application/json");
        _ = await client.PostAsync("https://httpbin.org/post", content);

        Check.That(testHandler).HasMadeRequests().WithContentHeader("content-type");
        Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type");
        Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "application/json; charset=utf-8");
        Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "application/json*");
        Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "*charset=utf-8");

        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Disposition")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "application/json")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContentHeader("Content-Type", "*=utf-16")).IsAFailingCheck();
    }

    [Fact]
    public async Task AssertingContent()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        using var content = new StringContent("my special content");
        _ = await client.PostAsync("https://httpbin.org/post", content);

#if NETFRAMEWORK
        // On .NET Framework the HttpClient disposes the content automatically. So we can't perform the same test.
        Check.That(testHandler).HasMadeRequests();
#else
        Check.That(testHandler).HasMadeRequests().WithContent("my special content");
        Check.That(testHandler).HasMadeRequests().WithContent("my*content");
        Check.That(testHandler).HasMadeRequests().WithContent("*");

        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContent("")).IsAFailingCheck();
        Check.ThatCode(() => Check.That(testHandler).HasMadeRequests().WithContent("my")).IsAFailingCheck();
#endif
    }

    [Fact]
    public async Task AssertJsonContent()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        using var content = new StringContent("{}", Encoding.UTF8, "application/json");
        _ = await client.PostAsync("https://httpbin.org/post", content);

#if NETFRAMEWORK
        // On .NET Framework the HttpClient disposes the content automatically. So we can't perform the same test.
        Check.That(testHandler).HasMadeRequests();
#else
        Check.That(testHandler).HasMadeRequests().WithJsonContent(new { });
#endif
    }

    [Fact]
    public async Task CustomAssertions()
    {
        using var testHandler = new TestableHttpMessageHandler();
        using var client = new HttpClient(testHandler);

        using var content = new StringContent("", Encoding.UTF8, "application/json");
        _ = await client.PostAsync("https://httpbin.org/post", content);

        Check.That(testHandler).HasMadeRequests().WithFilter(x => x.HasContentHeader("Content-Type", "application/json") || x.HasContentHeader("Content-Type", "application/json; *"), "");
    }
}
