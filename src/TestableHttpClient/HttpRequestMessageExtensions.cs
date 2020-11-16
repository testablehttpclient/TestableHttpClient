using System;
using System.Net.Http;

using TestableHttpClient.Utils;

namespace TestableHttpClient
{
    /// <summary>
    /// A set of static methods for checking values on a <see cref="HttpRequestMessage"/>.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Determines whether a specific HttpVersion is set on a request.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct version on.</param>
        /// <param name="httpVersion">The expected version.</param>
        /// <returns>true when the HttpVersion matches; otherwise, false.</returns>
        public static bool HasHttpVersion(this HttpRequestMessage httpRequestMessage, Version httpVersion)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (httpVersion == null)
            {
                throw new ArgumentNullException(nameof(httpVersion));
            }

            return httpRequestMessage.Version == httpVersion;
        }

        /// <summary>
        /// Determines whether a specific HttpVersion is set on a request.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct version on.</param>
        /// <param name="httpVersion">The expected version.</param>
        /// <returns>true when the HttpVersion matches; otherwise, false.</returns>
        public static bool HasHttpVersion(this HttpRequestMessage httpRequestMessage, string httpVersion)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (string.IsNullOrEmpty(httpVersion))
            {
                throw new ArgumentNullException(nameof(httpVersion));
            }

            return httpRequestMessage.HasHttpVersion(new Version(httpVersion));
        }

        /// <summary>
        /// Determines whether a specific HttpMethod is set on a request.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
        /// <param name="httpMethod">The expected method.</param>
        /// <returns>true when the HttpMethod matches; otherwise, false.</returns>
        public static bool HasHttpMethod(this HttpRequestMessage httpRequestMessage, HttpMethod httpMethod)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (httpMethod == null)
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            return httpRequestMessage.Method == httpMethod;
        }

        /// <summary>
        /// Determines whether a specific HttpMethod is set on a request.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
        /// <param name="httpMethod">The expected method.</param>
        /// <returns>true when the HttpMethod matches; otherwise, false.</returns>
        public static bool HasHttpMethod(this HttpRequestMessage httpRequestMessage, string httpMethod)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            return httpRequestMessage.HasHttpMethod(new HttpMethod(httpMethod));
        }

        /// <summary>
        /// Determines whether a specific header is set on a request.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpRequestHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the request header on.</param>
        /// <param name="headerName">The name of the header to locate on the request.</param>
        /// <returns>true when the request contains a header with the specified name; otherwise, false.</returns>
        public static bool HasRequestHeader(this HttpRequestMessage httpRequestMessage, string headerName)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            return httpRequestMessage.Headers.HasHeader(headerName);
        }

        /// <summary>
        /// Determines whether a specific header with a specific value is set on a request.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpRequestHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the request header on.</param>
        /// <param name="headerName">The name of the header to locate on the request.</param>
        /// <param name="headerValue">The value the header should have. Wildcard is supported.</param>
        /// <returns>true when the request contains a header with the specified name and value; otherwise, false.</returns>
        public static bool HasRequestHeader(this HttpRequestMessage httpRequestMessage, string headerName, string headerValue)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }

            return httpRequestMessage.Headers.HasHeader(headerName, headerValue);
        }

        /// <summary>
        /// Determines whether a specific header is set on a request.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpContentHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the content header on.</param>
        /// <param name="headerName">The name of the header to locate on the request content.</param>
        /// <returns>true when the request contains a header with the specified name; otherwise, false.</returns>
        public static bool HasContentHeader(this HttpRequestMessage httpRequestMessage, string headerName)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (httpRequestMessage.Content == null)
            {
                return false;
            }

            return httpRequestMessage.Content.Headers.HasHeader(headerName);
        }

        /// <summary>
        /// Determines whether a specific header with a specific value is set on a request.
        /// </summary>
        /// <remarks>This method only checks headers in <see cref="HttpContentHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the content header on.</param>
        /// <param name="headerName">The name of the header to locate on the request content.</param>
        /// <param name="headerValue">The value the header should have. Wildcard is supported.</param>
        /// <returns>true when the request contains a header with the specified name and value; otherwise, false.</returns>
        public static bool HasContentHeader(this HttpRequestMessage httpRequestMessage, string headerName, string headerValue)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }

            if (httpRequestMessage.Content == null)
            {
                return false;
            }

            return httpRequestMessage.Content.Headers.HasHeader(headerName, headerValue);
        }

        /// <summary>
        /// Determines whether the request uri matches a pattern.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct uri on.</param>
        /// <param name="pattern">A pattern to match with the request uri, supports * as wildcards.</param>
        /// <returns>true when the request uri matches the pattern; otherwise, false.</returns>
        public static bool HasMatchingUri(this HttpRequestMessage httpRequestMessage, string pattern)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            return pattern switch
            {
                null => throw new ArgumentNullException(nameof(pattern)),
                "" => false,
                "*" => true,
                _ => StringMatcher.Matches(httpRequestMessage.RequestUri.AbsoluteUri, pattern),
            };
        }

        /// <summary>
        /// Determines whether the request has content.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check for content.</param>
        /// <returns>true when the request has content; otherwise, false.</returns>
        public static bool HasContent(this HttpRequestMessage httpRequestMessage)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            return httpRequestMessage.Content != null;
        }

        /// <summary>
        /// Determines whether the request content matches a string pattern.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct content on.</param>
        /// <param name="pattern">A pattern to match the request content, supports * as wildcards.</param>
        /// <returns>true when the request content matches the pattern; otherwise, false.</returns>
        public static bool HasContent(this HttpRequestMessage httpRequestMessage, string pattern)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (httpRequestMessage.Content == null)
            {
                return false;
            }

            var stringContent = httpRequestMessage.Content.ReadAsStringAsync().Result;

            return pattern switch
            {
                "" => stringContent == pattern,
                "*" => true,
                _ => StringMatcher.Matches(stringContent, pattern),
            };
        }

        /// <summary>
        /// Determines whether the request uri querystring matches a string pattern. For asserting the decoded version of the querystring is used.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct request uir querystring on.</param>
        /// <param name="pattern">A pattern to match the request uri querystring, supports * as wildcards.</param>
        /// <returns>true when the request uri querystring matches the pattern; otherwise, false.</returns>
        public static bool HasQueryString(this HttpRequestMessage httpRequestMessage, string pattern)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var requestQuery = httpRequestMessage.RequestUri.GetComponents(UriComponents.Query, UriFormat.Unescaped);

            return StringMatcher.Matches(requestQuery, pattern);
        }
    }
}
