using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace HttpClientTestHelpers
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
        /// <remarks>This method only checks headers in <see cref="System.Net.Http.Headers.HttpRequestHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
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
        /// <remarks>This method only checks headers in <see cref="System.Net.Http.Headers.HttpRequestHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
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
        /// <remarks>This method only checks headers in <see cref="System.Net.Http.Headers.HttpContentHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
        /// <param name="headerName">The name of the header to locate on the request.</param>
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
        /// <remarks>This method only checks headers in <see cref="System.Net.Http.Headers.HttpContentHeaders"/></remarks>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
        /// <param name="headerName">The name of the header to locate on the request.</param>
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

        private static bool HasHeader(this HttpHeaders headers, string headerName)
        {
            return headers.Contains(headerName);
        }

        private static bool HasHeader(this HttpHeaders headers, string headerName, string headerValue)
        {
            if (headers.TryGetValues(headerName, out var values))
            {
                var value = string.Join(" ", values);
                return Matches(value, headerValue);
            }

            return false;
        }

        /// <summary>
        /// Determines whether the request uri matches a pattern.
        /// </summary>
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
        /// <param name="pattern">A pattern to match with the request uri, support * as wildcards.</param>
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
                _ => Matches(httpRequestMessage.RequestUri.AbsoluteUri, pattern),
            };
        }

        private static bool Matches(string value, string pattern)
        {
            var regex = Regex.Escape(pattern).Replace("\\*", "(.*)");

            return Regex.IsMatch(value, regex);
        }
    }
}
