﻿namespace TestableHttpClient;

public static class HttpRequestMessagesCheckExtensions
{
    /// <summary>
    /// Asserts whether requests were made to a given URI based on a pattern.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithRequestUri(this IHttpRequestMessagesCheck check, string pattern) => WithRequestUri(check, pattern, null);

    /// <summary>
    /// Asserts whether requests were made to a given URI based on a pattern.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="pattern">The uri pattern that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithRequestUri(this IHttpRequestMessagesCheck check, string pattern, int expectedNumberOfRequests) => WithRequestUri(check, pattern, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithRequestUri(this IHttpRequestMessagesCheck check, string pattern, int? expectedNumberOfRequests)
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

        return check.WithFilter(x => x.HasMatchingUri(pattern), expectedNumberOfRequests, condition);
    }

    /// <summary>
    /// Asserts whether requests were made with a given querystring based on a pattern. For asserting the decoded version of the querystring is used.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="pattern">The querystring pattern that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithQueryString(this IHttpRequestMessagesCheck check, string pattern) => WithQueryString(check, pattern, null);

    /// <summary>
    /// Asserts whether requests were made with a given querystring based on a pattern. For asserting the decoded version of the querystring is used.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="pattern">The querystring pattern that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithQueryString(this IHttpRequestMessagesCheck check, string pattern, int expectedNumberOfRequests) => WithQueryString(check, pattern, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithQueryString(this IHttpRequestMessagesCheck check, string pattern, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        var condition = pattern switch
        {
            "" => "no querystring",
            "*" => "any querystring",
            _ => $"querystring pattern '{pattern}'"
        };

        return check.WithFilter(x => x.HasQueryString(pattern), expectedNumberOfRequests, condition);
    }

    /// <summary>
    /// Asserts whether requests were made with a given HTTP Method.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHttpMethod(this IHttpRequestMessagesCheck check, HttpMethod httpMethod) => WithHttpMethod(check, httpMethod, null);

    /// <summary>
    /// Asserts whether requests were made with a given HTTP Method.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="httpMethod">The <seealso cref="HttpMethod"/> that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHttpMethod(this IHttpRequestMessagesCheck check, HttpMethod httpMethod, int expectedNumberOfRequests) => WithHttpMethod(check, httpMethod, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithHttpMethod(this IHttpRequestMessagesCheck check, HttpMethod httpMethod, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        if (httpMethod == null)
        {
            throw new ArgumentNullException(nameof(httpMethod));
        }

        return check.WithFilter(x => x.HasHttpMethod(httpMethod), expectedNumberOfRequests, $"HTTP Method '{httpMethod}'");
    }

    /// <summary>
    /// Asserts whether requests were made using a specific HTTP Version.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHttpVersion(this IHttpRequestMessagesCheck check, Version httpVersion) => WithHttpVersion(check, httpVersion, null);

    /// <summary>
    /// Asserts whether requests were made using a specific HTTP Version.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="httpVersion">The <seealso cref="System.Net.HttpVersion"/> that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHttpVersion(this IHttpRequestMessagesCheck check, Version httpVersion, int expectedNumberOfRequests) => WithHttpVersion(check, httpVersion, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithHttpVersion(this IHttpRequestMessagesCheck check, Version httpVersion, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        if (httpVersion == null)
        {
            throw new ArgumentNullException(nameof(httpVersion));
        }

        return check.WithFilter(x => x.HasHttpVersion(httpVersion), expectedNumberOfRequests, $"HTTP Version '{httpVersion}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName) => WithRequestHeader(check, headerName, (int?)null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, int expectedNumberOfRequests) => WithRequestHeader(check, headerName, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        if (string.IsNullOrEmpty(headerName))
        {
            throw new ArgumentNullException(nameof(headerName));
        }

        return check.WithFilter(x => x.HasRequestHeader(headerName), expectedNumberOfRequests, $"request header '{headerName}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue) => WithRequestHeader(check, headerName, headerValue, null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int expectedNumberOfRequests) => WithRequestHeader(check, headerName, headerValue, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int? expectedNumberOfRequests)
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

        return check.WithFilter(x => x.HasRequestHeader(headerName, headerValue), expectedNumberOfRequests, $"request header '{headerName}' and value '{headerValue}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName) => WithContentHeader(check, headerName, (int?)null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, int expectedNumberOfRequests) => WithContentHeader(check, headerName, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        if (string.IsNullOrEmpty(headerName))
        {
            throw new ArgumentNullException(nameof(headerName));
        }

        return check.WithFilter(x => x.HasContentHeader(headerName), expectedNumberOfRequests, $"content header '{headerName}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue) => WithContentHeader(check, headerName, headerValue, (int?)null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeader"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int expectedNumberOfRequests) => WithContentHeader(check, headerName, headerValue, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int? expectedNumberOfRequests)
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

        return check.WithFilter(x => x.HasContentHeader(headerName, headerValue), expectedNumberOfRequests, $"content header '{headerName}' and value '{headerValue}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName) => WithHeader(check, headerName, (int?)null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName, int expectedNumberOfRequests) => WithHeader(check, headerName, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        if (string.IsNullOrEmpty(headerName))
        {
            throw new ArgumentNullException(nameof(headerName));
        }

        return check.WithFilter(x => x.HasRequestHeader(headerName) || x.HasContentHeader(headerName), expectedNumberOfRequests, $"header '{headerName}'");
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue) => WithHeader(check, headerName, headerValue, (int?)null);

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int expectedNumberOfRequests) => WithHeader(check, headerName, headerValue, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int? expectedNumberOfRequests)
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

        return check.WithFilter(x => x.HasRequestHeader(headerName, headerValue) || x.HasContentHeader(headerName, headerValue), expectedNumberOfRequests, $"header '{headerName}' and value '{headerValue}'");
    }

    /// <summary>
    /// Asserts whether requests were made with specific content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="pattern">The expected content, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    /// <remarks>Note that on .NET Framework, the HttpClient might dispose the content after sending the request.</remarks>
    public static IHttpRequestMessagesCheck WithContent(this IHttpRequestMessagesCheck check, string pattern) => WithContent(check, pattern, null);

    /// <summary>
    /// Asserts whether requests were made with specific content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="pattern">The expected content, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    /// <remarks>Note that on .NET Framework, the HttpClient might dispose the content after sending the request.</remarks>
    public static IHttpRequestMessagesCheck WithContent(this IHttpRequestMessagesCheck check, string pattern, int expectedNumberOfRequests) => WithContent(check, pattern, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithContent(this IHttpRequestMessagesCheck check, string pattern, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        return check.WithFilter(x => x.HasContent(pattern), expectedNumberOfRequests, $"content '{pattern}'");
    }

    /// <summary>
    /// Asserts wheter requests are made with specific json content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="jsonObject">The object representation of the expected request content.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    /// <remarks>Note that on .NET Framework, the HttpClient might dispose the content after sending the request.</remarks>
    public static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject) => WithJsonContent(check, jsonObject, null);

    /// <summary>
    /// Asserts wheter requests are made with specific json content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="jsonObject">The object representation of the expected request content.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    /// <remarks>Note that on .NET Framework, the HttpClient might dispose the content after sending the request.</remarks>
    public static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject, int expectedNumberOfRequests) => WithJsonContent(check, jsonObject, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject, int? expectedNumberOfRequests)
    {
        if (check == null)
        {
            throw new ArgumentNullException(nameof(check));
        }

        var jsonString = JsonSerializer.Serialize(jsonObject);
        return check.WithFilter(x => x.HasContent(jsonString) && x.HasContentHeader("Content-Type", "application/json*"), expectedNumberOfRequests, $"json content '{jsonString}'");
    }

    /// <summary>
    /// Asserts wheter requests are made with specific url encoded content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="nameValueCollection">The collection of key/value pairs that should be url encoded.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    /// <remarks>Note that on .NET Framework, the HttpClient might dispose the content after sending the request.</remarks>
    public static IHttpRequestMessagesCheck WithFormUrlEncodedContent(this IHttpRequestMessagesCheck check, IEnumerable<KeyValuePair<string?, string?>> nameValueCollection) => WithFormUrlEncodedContent(check, nameValueCollection, null);

    /// <summary>
    /// Asserts wheter requests are made with specific url encoded content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="nameValueCollection">The collection of key/value pairs that should be url encoded.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    /// <remarks>Note that on .NET Framework, the HttpClient might dispose the content after sending the request.</remarks>
    public static IHttpRequestMessagesCheck WithFormUrlEncodedContent(this IHttpRequestMessagesCheck check, IEnumerable<KeyValuePair<string?, string?>> nameValueCollection, int expectedNumberOfRequests) => WithFormUrlEncodedContent(check, nameValueCollection, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithFormUrlEncodedContent(this IHttpRequestMessagesCheck check, IEnumerable<KeyValuePair<string?, string?>> nameValueCollection, int? expectedNumberOfRequests)
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

        return check.WithFilter(x => x.HasContent(contentString) && x.HasContentHeader("Content-Type", "application/x-www-form-urlencoded*"), expectedNumberOfRequests, $"form url encoded content '{contentString}'");
    }
}
