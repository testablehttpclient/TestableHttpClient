using System;
using System.Net;
using System.Net.Http;

namespace HttpClientTestHelpers
{
    public class HttpResponseMessageBuilder
    {
        private readonly HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

        public HttpResponseMessageBuilder WithVersion(Version httpVersion)
        {
            httpResponseMessage.Version = httpVersion;
            return this;
        }

        public HttpResponseMessageBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            httpResponseMessage.StatusCode = statusCode;
            return this;
        }

        public HttpResponseMessageBuilder WithHeader(string header, string value)
        {
            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentNullException(nameof(header));
            }

            httpResponseMessage.Headers.Add(header, value);
            return this;
        }

        public HttpResponseMessageBuilder WithContent(HttpContent content)
        {
            httpResponseMessage.Content = content;
            return this;
        }

        public HttpResponseMessageBuilder WithRequestMessage(HttpRequestMessage requestMessage)
        {
            httpResponseMessage.RequestMessage = requestMessage;
            return this;
        }

        public HttpResponseMessage Build()
        {
            return httpResponseMessage;
        }
    }
}
