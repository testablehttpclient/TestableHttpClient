using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace TestableHttpClient
{
    public static class HttpRequestMessagesCheckExtensions
    {
        /// <summary>
        /// Asserts whether requests were made to a given URI based on a pattern.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="pattern">The uri pattern that is expected.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        [Obsolete("Renamed to WithRequestUri")]
        public static IHttpRequestMessagesCheck WithUriPattern(this IHttpRequestMessagesCheck check, string pattern)
        {
            return WithRequestUri(check, pattern);
        }

        /// <summary>
        /// Asserts whether requests were made to a given URI based on a pattern.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="pattern">The uri pattern that is expected.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithRequestUri(this IHttpRequestMessagesCheck check, string pattern)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var condition = string.Empty;
            if (pattern != "*")
            {
                condition = $"uri pattern '{pattern}'";
            }

            return check.With(x => x.HasMatchingUri(pattern), condition);
        }

        /// <summary>
        /// Asserts whether requests were made with a given HTTP Method.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithHttpMethod(this IHttpRequestMessagesCheck check, HttpMethod httpMethod)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (httpMethod == null)
            {
                throw new ArgumentNullException(nameof(httpMethod));
            }

            return check.With(x => x.HasHttpMethod(httpMethod), $"HTTP Method '{httpMethod}'");
        }

        /// <summary>
        /// Asserts whether requests were made using a specific HTTP Version.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithHttpVersion(this IHttpRequestMessagesCheck check, Version httpVersion)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (httpVersion == null)
            {
                throw new ArgumentNullException(nameof(httpVersion));
            }

            return check.With(x => x.HasHttpVersion(httpVersion), $"HTTP Version '{httpVersion}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name. Values are ignored.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            return check.With(x => x.HasRequestHeader(headerName), $"request header '{headerName}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name and value.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }

            return check.With(x => x.HasRequestHeader(headerName, headerValue), $"request header '{headerName}' and value '{headerValue}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name. Values are ignored.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            return check.With(x => x.HasContentHeader(headerName), $"content header '{headerName}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name and value.
        /// </summary>
        /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }

            return check.With(x => x.HasContentHeader(headerName, headerValue), $"content header '{headerName}' and value '{headerValue}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name. Values are ignored.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            return check.With(x => x.HasRequestHeader(headerName) || x.HasContentHeader(headerName), $"header '{headerName}'");
        }

        /// <summary>
        /// Asserts whether requests were made with a specific header name and value.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="headerName">The name of the header that is expected.</param>
        /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (string.IsNullOrEmpty(headerName))
            {
                throw new ArgumentNullException(nameof(headerName));
            }

            if (string.IsNullOrEmpty(headerValue))
            {
                throw new ArgumentNullException(nameof(headerValue));
            }

            return check.With(x => x.HasRequestHeader(headerName, headerValue) || x.HasContentHeader(headerName, headerValue), $"header '{headerName}' and value '{headerValue}'");
        }

        /// <summary>
        /// Asserts whether requests were made with specific content.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="pattern">The expected content, supports wildcards.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithContent(this IHttpRequestMessagesCheck check, string pattern)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return check.With(x => x.HasContent(pattern), $"content '{pattern}'");
        }

        /// <summary>
        /// Asserts wheter requests are made with specific json content.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="jsonObject">The object representation of the expected request content.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            var jsonString = JsonSerializer.Serialize(jsonObject);
            return check.With(x => x.HasContent(jsonString) && x.HasContentHeader("Content-Type", "application/json*"), $"json content '{jsonString}'");
        }

        /// <summary>
        /// Asserts wheter requests are made with specific url encoded content.
        /// </summary>
        /// <param name="check">The implementation that hold all the request messages.</param>
        /// <param name="nameValueCollection">The collection of key/value pairs that should be url encoded.</param>
        /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
        public static IHttpRequestMessagesCheck WithFormUrlEncodedContent(this IHttpRequestMessagesCheck check, IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (check == null)
            {
                throw new ArgumentNullException(nameof(check));
            }

            if (nameValueCollection == null)
            {
                throw new ArgumentNullException(nameof(nameValueCollection));
            }

            using var content = new FormUrlEncodedContent(nameValueCollection);
            var contentString = content.ReadAsStringAsync().Result;

            return check.With(x => x.HasContent(contentString) && x.HasContentHeader("Content-Type", "application/x-www-form-urlencoded*"), $"form url encoded content '{contentString}'");
        }
    }
}
