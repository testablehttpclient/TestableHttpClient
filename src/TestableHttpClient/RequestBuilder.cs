namespace TestableHttpClient;

public sealed class RequestBuilder
{
    private Request request;
    internal TestableHttpMessageHandlerOptions Options { get; }

    internal RequestBuilder(TestableHttpMessageHandlerOptions? options = null)
    {
        Options = options ?? new();
        request = new(Options.UriPatternMatchingOptions);
    }

    public RequestBuilder WithMethod(HttpMethod httpMethod)
    {
        Guard.ThrowIfNull(httpMethod);

        request = request with { Method = httpMethod };
        return this;
    }

    public RequestBuilder WithRequestUri(string pattern)
    {
        Guard.ThrowIfNullOrEmpty(pattern);

        request = request with { RequestUri = UriPatternParser.Parse(pattern) };
        return this;
    }

    public RequestBuilder WithVersion(Version httpVersion)
    {
        Guard.ThrowIfNull(httpVersion);

        request = request with { Version = httpVersion };
        return this;
    }

    public RequestBuilder WithHeader(string headerName)
    {
        Guard.ThrowIfNullOrEmpty(headerName);

        request = request.AddHeader(headerName, Value.Any());
        return this;
    }

    public RequestBuilder WithHeader(string headerName, string pattern)
    {
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(pattern);

        request = request.AddHeader(headerName, Value.Pattern(pattern));
        return this;
    }

    public RequestBuilder WithContent(string pattern)
    {
        Guard.ThrowIfNull(pattern);

        request = request with { Content = pattern };
        return this;
    }

    internal Request Build() => request;
}
