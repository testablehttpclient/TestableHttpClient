namespace TestableHttpClient;

/// <summary>
/// A set of static methods for checking values on a <see cref="HttpRequestMessage"/>.
/// </summary>
internal static class HttpRequestMessageExtensions
{
    /// <summary>
    /// Determines whether a specific HttpVersion is set on a request.
    /// </summary>
    /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct version on.</param>
    /// <param name="httpVersion">The expected version.</param>
    /// <returns>true when the HttpVersion matches; otherwise, false.</returns>
    internal static bool HasHttpVersion(this HttpRequestMessage httpRequestMessage, Version httpVersion)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNull(httpVersion);

        return httpRequestMessage.Version == httpVersion;
    }

    /// <summary>
    /// Determines whether a specific HttpMethod is set on a request.
    /// </summary>
    /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct method on.</param>
    /// <param name="httpMethod">The expected method.</param>
    /// <returns>true when the HttpMethod matches; otherwise, false.</returns>
    internal static bool HasHttpMethod(this HttpRequestMessage httpRequestMessage, HttpMethod httpMethod)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNull(httpMethod);

        return httpRequestMessage.Method == httpMethod;
    }

    internal static bool HasHeader(this HttpRequestMessage httpRequestMessage, string headerName)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);

        return httpRequestMessage.Headers.HasHeader(headerName) || (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.HasHeader(headerName));
    }

    internal static bool HasHeader(this HttpRequestMessage httpRequestMessage, string headerName, string headerValue)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(headerValue);

        return httpRequestMessage.Headers.HasHeader(headerName, headerValue) || (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.HasHeader(headerName, headerValue));
    }

    [Obsolete("Use HasHeader instead.")]
    internal static bool HasRequestHeader(this HttpRequestMessage httpRequestMessage, string headerName)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);

        return httpRequestMessage.Headers.HasHeader(headerName);
    }

    [Obsolete("Use HasHeader instead.")]
    internal static bool HasRequestHeader(this HttpRequestMessage httpRequestMessage, string headerName, string headerValue)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(headerValue);

        return httpRequestMessage.Headers.HasHeader(headerName, headerValue);
    }

    [Obsolete("Use HasHeader instead.")]
    internal static bool HasContentHeader(this HttpRequestMessage httpRequestMessage, string headerName)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);

        if (httpRequestMessage.Content == null)
        {
            return false;
        }

        return httpRequestMessage.Content.Headers.HasHeader(headerName);
    }

    [Obsolete("Use HasHeader instead.")]
    internal static bool HasContentHeader(this HttpRequestMessage httpRequestMessage, string headerName, string headerValue)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(headerValue);

        if (httpRequestMessage.Content == null)
        {
            return false;
        }

        return httpRequestMessage.Content.Headers.HasHeader(headerName, headerValue);
    }

    /// <summary>
    /// Determines whether the request content matches a string pattern.
    /// </summary>
    /// <param name="httpRequestMessage">A <see cref="HttpRequestMessage"/> to check the correct content on.</param>
    /// <param name="pattern">A pattern to match the request content, supports * as wildcards.</param>
    /// <returns>true when the request content matches the pattern; otherwise, false.</returns>
    internal static bool HasContent(this HttpRequestMessage httpRequestMessage, string pattern)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNull(pattern);

        if (httpRequestMessage.Content == null)
        {
            return false;
        }

        var stringContent = httpRequestMessage.Content.ReadAsStringAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();

        return pattern switch
        {
            "" => stringContent == pattern,
            "*" => true,
            _ => StringMatcher.Matches(stringContent, pattern),
        };
    }
}
