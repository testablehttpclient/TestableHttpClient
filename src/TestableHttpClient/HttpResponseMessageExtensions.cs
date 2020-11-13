using System;
using System.Net;
using System.Net.Http;

using TestableHttpClient.Utils;

namespace TestableHttpClient
{
    /// <summary>
    /// A set of static methods for checking values on a <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Determines whether a specific HttpVersion is set on a response.
        /// </summary>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the correct version on.</param>
        /// <param name="httpVersion">The expected version.</param>
        /// <returns>true when the HttpVersion matches; otherwise, false.</returns>
        public static bool HasHttpVersion(this HttpResponseMessage httpResponseMessage, Version httpVersion)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (httpVersion == null)
            {
                throw new ArgumentNullException(nameof(httpVersion));
            }

            return httpResponseMessage.Version == httpVersion;
        }

        /// <summary>
        /// Determines whether a specific status code is set on a response.
        /// </summary>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the correct version on.</param>
        /// <param name="httpStatusCode">The expected status code.</param>
        /// <returns>true when the status code matches; otherwise, false.</returns>
        public static bool HasHttpStatusCode(this HttpResponseMessage httpResponseMessage, HttpStatusCode httpStatusCode)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            return httpResponseMessage.StatusCode == httpStatusCode;
        }

        /// <summary>
        /// Determines whether a specific reason phrase is set on a response.
        /// </summary>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the correct version on.</param>
        /// <param name="reasonPhrase">The expected reason phrase.</param>
        /// <returns>true when the reason phrase matches; otherwise, false.</returns>
        public static bool HasReasonPhrase(this HttpResponseMessage httpResponseMessage, string reasonPhrase)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (reasonPhrase == null)
            {
                throw new ArgumentNullException(nameof(reasonPhrase));
            }

            return httpResponseMessage.ReasonPhrase == reasonPhrase;
        }

        /// <summary>
        /// Determines whether a specific header is set on a response.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpRequestHeaders"/></remarks>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the response header on.</param>
        /// <param name="headerName">The name of the header to locate on the response.</param>
        /// <returns>true when the response contains a header with the specified name; otherwise, false.</returns>
        public static bool HasResponseHeader(this HttpResponseMessage httpResponseMessage, string headerName)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            return httpResponseMessage.Headers.HasHeader(headerName);
        }

        /// <summary>
        /// Determines whether a specific header with a specific value is set on a response.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpRequestHeaders"/></remarks>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the response header on.</param>
        /// <param name="headerName">The name of the header to locate on the response.</param>
        /// <param name="headerValue">The value the header should have. Wildcard is supported.</param>
        /// <returns>true when the response contains a header with the specified name and value; otherwise, false.</returns>
        public static bool HasResponseHeader(this HttpResponseMessage httpResponseMessage, string headerName, string headerValue)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }

            return httpResponseMessage.Headers.HasHeader(headerName, headerValue);
        }

        /// <summary>
        /// Determines whether a specific header is set on a response.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpContentHeaders"/></remarks>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the content header on.</param>
        /// <param name="headerName">The name of the header to locate on the response content.</param>
        /// <returns>true when the response contains a header with the specified name; otherwise, false.</returns>
        public static bool HasContentHeader(this HttpResponseMessage httpResponseMessage, string headerName)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (httpResponseMessage.Content == null)
            {
                return false;
            }

            return httpResponseMessage.Content.Headers.HasHeader(headerName);
        }

        /// <summary>
        /// Determines whether a specific header with a specific value is set on a response.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpContentHeaders"/></remarks>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the content header on.</param>
        /// <param name="headerName">The name of the header to locate on the response content.</param>
        /// <param name="headerValue">The value the header should have. Wildcard is supported.</param>
        /// <returns>true when the response contains a header with the specified name and value; otherwise, false.</returns>
        public static bool HasContentHeader(this HttpResponseMessage httpResponseMessage, string headerName, string headerValue)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }

            if (httpResponseMessage.Content == null)
            {
                return false;
            }

            return httpResponseMessage.Content.Headers.HasHeader(headerName, headerValue);
        }

        /// <summary>
        /// Determines whether the response has content.
        /// </summary>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check for content.</param>
        /// <returns>true when the response has content; otherwise, false.</returns>
        public static bool HasContent(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (httpResponseMessage.Content == null)
            {
                return false;
            }

            var stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
            return stream.ReadByte() != -1;
        }

        /// <summary>
        /// Determines whether the response content matches a string pattern.
        /// </summary>
        /// <param name="httpResponseMessage">A <see cref="HttpResponseMessage"/> to check the correct content on.</param>
        /// <param name="pattern">A pattern to match the response content, supports * as wildcards.</param>
        /// <returns>true when the response content matches the pattern; otherwise, false.</returns>
        public static bool HasContent(this HttpResponseMessage httpResponseMessage, string pattern)
        {
            if (httpResponseMessage == null)
            {
                throw new ArgumentNullException(nameof(httpResponseMessage));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var stringContent = httpResponseMessage.Content?.ReadAsStringAsync()?.Result ?? string.Empty;

            return pattern switch
            {
                "" => stringContent == pattern,
                "*" => true,
                _ => StringMatcher.Matches(stringContent, pattern),
            };
        }
    }
}
