namespace TestableHttpClient;

public static class HttpRequestMessagesCheckExtensions
{
    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName);
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, int expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName, expectedNumberOfRequests);
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName, headerValue);
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpRequestHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithRequestHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName, headerValue, expectedNumberOfRequests);
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName);
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name. Values are ignored.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, int expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName, expectedNumberOfRequests);
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName, headerValue);
    }

    /// <summary>
    /// Asserts whether requests were made with a specific header name and value.
    /// </summary>
    /// <remarks>This method only asserts headers on <see cref="System.Net.Http.Headers.HttpContentHeaders"/></remarks>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="headerName">The name of the header that is expected.</param>
    /// <param name="headerValue">The value of the expected header, supports wildcards.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    [Obsolete("Use WithHeader instead.")]
    public static IHttpRequestMessagesCheck WithContentHeader(this IHttpRequestMessagesCheck check, string headerName, string headerValue, int expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(check);

        return check.WithHeader(headerName, headerValue, expectedNumberOfRequests);
    }

    /// <summary>
    /// Asserts whether requests are made with specific json content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="jsonObject">The object representation of the expected request content.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject) => WithJsonContent(check, jsonObject, null, null);

    /// <summary>
    /// Asserts whether requests are made with specific json content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="jsonObject">The object representation of the expected request content.</param>
    /// <param name="jsonSerializerOptions">The serializer options that should be used for serializing te content.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject, JsonSerializerOptions jsonSerializerOptions) => WithJsonContent(check, jsonObject, jsonSerializerOptions, null);

    /// <summary>
    /// Asserts whether requests are made with specific json content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="jsonObject">The object representation of the expected request content.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject, int expectedNumberOfRequests) => WithJsonContent(check, jsonObject, null, (int?)expectedNumberOfRequests);

    /// <summary>
    /// Asserts whether requests are made with specific json content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="jsonObject">The object representation of the expected request content.</param>
    /// <param name="jsonSerializerOptions">The serializer options that should be used for serializing the content.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject, JsonSerializerOptions jsonSerializerOptions, int expectedNumberOfRequests) => WithJsonContent(check, jsonObject, jsonSerializerOptions, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithJsonContent(this IHttpRequestMessagesCheck check, object? jsonObject, JsonSerializerOptions? jsonSerializerOptions, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(check);

        var jsonString = JsonSerializer.Serialize(jsonObject, jsonSerializerOptions ?? check.Options.JsonSerializerOptions);

        return check.WithFilter(x => x.HasContent(jsonString) && x.HasHeader("Content-Type", "application/json*"), expectedNumberOfRequests, $"json content '{jsonString}'");
    }

    /// <summary>
    /// Asserts whether requests are made with specific url encoded content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="nameValueCollection">The collection of key/value pairs that should be url encoded.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithFormUrlEncodedContent(this IHttpRequestMessagesCheck check, IEnumerable<KeyValuePair<string?, string?>> nameValueCollection) => WithFormUrlEncodedContent(check, nameValueCollection, null);

    /// <summary>
    /// Asserts whether requests are made with specific url encoded content.
    /// </summary>
    /// <param name="check">The implementation that hold all the request messages.</param>
    /// <param name="nameValueCollection">The collection of key/value pairs that should be url encoded.</param>
    /// <param name="expectedNumberOfRequests">The expected number of requests.</param>
    /// <returns>The <seealso cref="IHttpRequestMessagesCheck"/> for further assertions.</returns>
    public static IHttpRequestMessagesCheck WithFormUrlEncodedContent(this IHttpRequestMessagesCheck check, IEnumerable<KeyValuePair<string?, string?>> nameValueCollection, int expectedNumberOfRequests) => WithFormUrlEncodedContent(check, nameValueCollection, (int?)expectedNumberOfRequests);

    private static IHttpRequestMessagesCheck WithFormUrlEncodedContent(this IHttpRequestMessagesCheck check, IEnumerable<KeyValuePair<string?, string?>> nameValueCollection, int? expectedNumberOfRequests)
    {
        Guard.ThrowIfNull(check);
        Guard.ThrowIfNull(nameValueCollection);

        using var content = new FormUrlEncodedContent(nameValueCollection);
        var contentString = content.ReadAsStringAsync().Result;

        return check.WithFilter(x => x.HasContent(contentString) && x.HasHeader("Content-Type", "application/x-www-form-urlencoded*"), expectedNumberOfRequests, $"form url encoded content '{contentString}'");
    }
}
