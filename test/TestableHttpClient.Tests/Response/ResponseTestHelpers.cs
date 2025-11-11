namespace TestableHttpClient.Tests.Response;

internal static class ResponseTestHelpers
{
    public static Task<HttpResponseMessage> TestAsync(this IResponse response) => TestAsync(response, "http://httpbin.org");
    public static async Task<HttpResponseMessage> TestAsync(this IResponse response, string url)
    {
        using TestableHttpMessageHandler handler = new();
        handler.RespondWith(response);
        return await handler.TestAsync(url);
    }

    public static Task<HttpResponseMessage> TestAsync(this TestableHttpMessageHandler handler) => TestAsync(handler, "http://httpbin.org");
    public static async Task<HttpResponseMessage> TestAsync(this TestableHttpMessageHandler handler, string url)
    {
        using HttpClient client = new(handler);
        return await client.GetAsync(url);
    }
}
