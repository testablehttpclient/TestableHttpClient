using System;
using System.Net.Http;

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
        /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
        /// <param name="headerName">The name of the header to locate on the request.</param>
        /// <returns>true when the request contains a header with the specified name; otherwise, false.</returns>
        public static bool HasHeader(this HttpRequestMessage httpRequestMessage, string headerName)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException(nameof(httpRequestMessage));
            }

            if(string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            return httpRequestMessage.Headers.Contains(headerName);
        }
    }
}
