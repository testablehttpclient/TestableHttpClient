namespace TestableHttpClient.Tests.Response;

public static class ResponseTestHelpers
{
    public static Task<HttpResponseMessage> TestAsync(this IResponse response) => TestAsync(response, "http://httpbin.org");
    public static Task<HttpResponseMessage> TestAsync(this IResponse response, string url)
    {
        using TestableHttpMessageHandler handler = new();
        handler.RespondWith(response);
        return handler.TestAsync(url);
    }

    public static Task<HttpResponseMessage> TestAsync(this TestableHttpMessageHandler handler) => TestAsync(handler, "http://httpbin.org");
    public static Task<HttpResponseMessage> TestAsync(this TestableHttpMessageHandler handler, string url)
    {
        using HttpClient client = new(handler);
        return client.GetAsync(url);
    }
}
