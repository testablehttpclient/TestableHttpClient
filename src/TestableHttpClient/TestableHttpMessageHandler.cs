using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TestableHttpClient
{
    /// <summary>
    /// A testable HTTP message handler that captures all requests and always returns the same response.
    /// </summary>
    public class TestableHttpMessageHandler : HttpMessageHandler
    {
        private readonly ConcurrentQueue<HttpRequestMessage> httpRequestMessages = new ConcurrentQueue<HttpRequestMessage>();
        private HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(string.Empty)
        };

        /// <summary>
        /// Gets the collection of captured requests made using this HttpMessageHandler.
        /// </summary>
        public IEnumerable<HttpRequestMessage> Requests => httpRequestMessages;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            httpRequestMessages.Enqueue(request);

            if (response is TimeoutHttpResponseMessage)
            {
                throw new TaskCanceledException(new OperationCanceledException().Message);
            }
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

        /// <summary>
        /// Configure the <see cref="HttpResponseMessage"/> that should be returned for each request using a <see cref="HttpResponseMessageBuilder"/>.
        /// </summary>
        /// <param name="httpResponseMessageBuilderAction">An action that calls methods on the <see cref="HttpResponseMessageBuilder"/>.</param>
        public void RespondWith(Action<HttpResponseMessageBuilder> httpResponseMessageBuilderAction)
        {
            if (httpResponseMessageBuilderAction == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessageBuilderAction));
            }

            var builder = new HttpResponseMessageBuilder();
            httpResponseMessageBuilderAction(builder);
            response = builder.Build();
        }

        /// <summary>
        /// Simulate a timeout on the request by throwing a TaskCanceledException when a request is received.
        /// </summary>
        public void SimulateTimeout()
        {
            response = new TimeoutHttpResponseMessage();
        }

        /// <summary>
        /// Validates that requests have been made, throws an exception when no requests were made.
        /// </summary>
        public HttpRequestMessageAsserter ShouldHaveMadeRequests()
        {
            return new HttpRequestMessageAsserter(Requests).WithUriPattern("*");
        }

        /// <summary>
        /// Validates that requests to a specific uri have been made, throws an exception when no requests were made.
        /// </summary>
        /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
        /// <returns>An <seealso cref="HttpRequestMessageAsserter"/> which can be used for further validations.</returns>
        public HttpRequestMessageAsserter ShouldHaveMadeRequestsTo(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return new HttpRequestMessageAsserter(Requests).WithUriPattern(pattern);
        }

        /// <summary>
        /// Validates that no requests have been made, throws an exception when requests were made.
        /// </summary>
        public void ShouldNotHaveMadeRequests()
        {
            _ = new HttpRequestMessageAsserter(Requests, true).WithUriPattern("*");
        }

        /// <summary>
        /// Validates that no requests to a specific uri have been made, throws an exception when requests were made.
        /// </summary>
        /// <param name="pattern">The uri pattern to validate against, the pattern supports *.</param>
        /// <returns>An <seealso cref="HttpRequestMessageAsserter"/> which can be used for further validations.</returns>
        public void ShouldNotHaveMadeRequestsTo(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            _ = new HttpRequestMessageAsserter(Requests, true).WithUriPattern(pattern);
        }

        private class TimeoutHttpResponseMessage : HttpResponseMessage
        {
        }
    }
}
