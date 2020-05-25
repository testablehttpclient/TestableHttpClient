using System;
using System.Net.Http;

namespace TestableHttpClient
{
    public static class TestableHttpMessageHandlerExtensions
    {
        /// <summary>
        /// Create an <seealso cref="HttpClient"/> configured with the TestableHttpMessageHandler.
        /// </summary>
        /// <param name="handler">The TestableHttpMessageHandler to set on the client.</param>
        /// <returns>An HttpClient configure with the TestableHttpMessageHandler.</returns>
        /// <exception cref="ArgumentNullException">The `handler` is `null`</exception>
        /// <remarks>Using this method is equivalent to `new HttClient(handler)`.</remarks>
        public static HttpClient CreateClient(this TestableHttpMessageHandler handler)
        {
            return CreateClient(handler, _ => { });
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
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (configureClient is null)
            {
                throw new ArgumentNullException(nameof(configureClient));
            }

            var httpClient = new HttpClient(handler);
            configureClient(httpClient);

            return httpClient;
        }
    }
}
