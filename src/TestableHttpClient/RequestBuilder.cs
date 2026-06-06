namespace TestableHttpClient;

/// <summary>
/// Class to build a request, used for assertions.
/// </summary>
public sealed class RequestBuilder
{
    private Request request;

    internal TestableHttpMessageHandlerOptions Options { get; }

    internal RequestBuilder(TestableHttpMessageHandlerOptions? options = null)
    {
        Options = options ?? new();
        request = new(Options.UriPatternMatchingOptions);
    }

    /// <summary>
    /// Specify the HTTP Method of a request, if not specified any method is matched.
    /// </summary>
    /// <param name="httpMethod">The expected HTTP Method.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder WithMethod(HttpMethod httpMethod)
    {
        Guard.ThrowIfNull(httpMethod);

        request = request with { Method = httpMethod };
        return this;
    }

    /// <summary>
    /// Specify the expected request uri, this can be a pattern, when not given any uri is used.
    /// </summary>
    /// <param name="pattern">The expected pattern of the request uri.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder WithRequestUri(string pattern)
    {
        Guard.ThrowIfNullOrEmpty(pattern);

        request = request with { RequestUri = UriPatternParser.Parse(pattern) };
        return this;
    }

    /// <summary>
    /// Specify the expected HTTP Version used for the request, if not specified any version is matched.
    /// </summary>
    /// <param name="httpVersion">The expected HTTP Version.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder WithVersion(Version httpVersion)
    {
        Guard.ThrowIfNull(httpVersion);

        request = request with { Version = httpVersion };
        return this;
    }

    /// <summary>
    /// Specify the headername that should be present in the request, the value is matched with the any value.
    /// </summary>
    /// <param name="headerName">The expected header name.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder WithHeader(string headerName) => WithHeader(headerName, "*");

    /// <summary>
    /// Specify the header name and value that should be present in the request, the value can be a pattern.
    /// </summary>
    /// <param name="headerName">The expected header name.</param>
    /// <param name="pattern">The expected value.</param>
    /// <returns>The request builder.</returns>
    public RequestBuilder WithHeader(string headerName, string pattern)
    {
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(pattern);

        request = request.AddHeader(headerName, Value.Pattern(pattern));
        return this;
    }

    /// <summary>
    /// Specify the content that should be present in the request, if not specified any content is valid.
    /// </summary>
    /// <param name="pattern">The expected content, this can be a pattern.</param>
    /// <returns>The request value</returns>
    public RequestBuilder WithContent(string pattern)
    {
        Guard.ThrowIfNull(pattern);

        request = request with { Content = pattern };
        return this;
    }

    internal Request Build() => request;
}
