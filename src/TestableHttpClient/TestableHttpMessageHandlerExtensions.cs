namespace TestableHttpClient;

public static class TestableHttpMessageHandlerExtensions
{
    private const string DefaultBaseAddress = "https://localhost";

    /// <summary>
    /// Create an <seealso cref="HttpClient"/> configured with the TestableHttpMessageHandler.
    /// </summary>
    /// <param name="handler">The TestableHttpMessageHandler to set on the client.</param>
    /// <returns>An HttpClient configure with the TestableHttpMessageHandler.</returns>
    /// <exception cref="ArgumentNullException">The `handler` is `null`</exception>
    /// <remarks>Using this method is equivalent to `new HttClient(handler)`.</remarks>
    public static HttpClient CreateClient(this TestableHttpMessageHandler handler, params DelegatingHandler[] httpMessageHandlers)
    {
        return CreateClient(handler, _ => { }, httpMessageHandlers);
    }

    /// <summary>
    /// Create and configure an <seealso cref="HttpClient"/> configured with the TestableHttpMessageHandler.
    /// </summary>
    /// <param name="handler">The TestableHttpMessageHandler to set on the client.</param>
    /// <param name="configureClient">A delegate that is used to configure an HttpClient</param>
    /// <returns>An HttpClient configure with the TestableHttpMessageHandler.</returns>
    /// <exception cref="ArgumentNullException">The `handler` or `configureClient` is `null`</exception>
    public static HttpClient CreateClient(this TestableHttpMessageHandler handler, Action<HttpClient> configureClient)
    {
        return CreateClient(handler, configureClient, Enumerable.Empty<DelegatingHandler>());
    }

    public static HttpClient CreateClient(this TestableHttpMessageHandler handler, Action<HttpClient> configureClient, IEnumerable<DelegatingHandler> httpMessageHandlers)
    {
        Guard.ThrowIfNull(handler);
        Guard.ThrowIfNull(configureClient);
        Guard.ThrowIfNull(httpMessageHandlers);

        if (httpMessageHandlers.Any(x => x is null))
        {
            throw new ArgumentNullException(nameof(httpMessageHandlers));
        }

        HttpClient httpClient = new(CreateHandlerChain(handler, httpMessageHandlers))
        {
            BaseAddress = new Uri(DefaultBaseAddress)
        };
        configureClient(httpClient);

        return httpClient;
    }

    private static HttpMessageHandler CreateHandlerChain(TestableHttpMessageHandler handler, IEnumerable<DelegatingHandler> additionalHandlers)
    {
        HttpMessageHandler next = handler;
        var reversedHandlers = additionalHandlers.Reverse();
        foreach (var delegatingHandler in reversedHandlers)
        {
            delegatingHandler.InnerHandler = next;
            next = delegatingHandler;
        }
        return next;
    }
}
