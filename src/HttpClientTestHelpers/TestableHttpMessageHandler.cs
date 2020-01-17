using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientTestHelpers
{
    /// <summary>
    /// A testable HTTP message handler that captures all requests and always returns the same response.
    /// </summary>
    public class TestableHttpMessageHandler : HttpMessageHandler
    {
        private readonly ConcurrentQueue<HttpRequestMessage> httpRequestMessages = new ConcurrentQueue<HttpRequestMessage>();
        private HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

        /// <summary>
        /// Gets the collection of captured requests made using this HttpMessageHandler.
        /// </summary>
        public IEnumerable<HttpRequestMessage> Requests => httpRequestMessages;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            httpRequestMessages.Enqueue(request);
            return Task.FromResult(response);
        }

        /// <summary>
        /// Configure the <see cref="HttpResponseMessage"/> that should be returned for each request.
        /// </summary>
        /// <param name="httpResponseMessage"></param>
        public void RespondWith(HttpResponseMessage httpResponseMessage)
        {
            response = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
        }
    }
}
