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
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return new HttpClient(handler);
        }
    }
}
