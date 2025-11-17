namespace TestableHttpClient;

internal record Request : IEquatable<HttpRequestMessage>
{
    public Request(UriPatternMatchingOptions uriPatternMatchingOptions)
    {
        UriPatternMatchingOptions = uriPatternMatchingOptions;
    }

    public UriPatternMatchingOptions UriPatternMatchingOptions { get; }

    public HttpMethod? HttpMethod { get; init; }
    public UriPattern? RequestUri { get; init; }
    public Version? HttpVersion { get; init; }

    public string? Content { get; init; }

    public bool Equals(HttpRequestMessage? other)
    {
        if (other is null)
        {
            return false;
        }

        if (HttpMethod is not null && other.Method != HttpMethod)
        {
            return false;
        }

        if (RequestUri is not null && other.RequestUri is not null && !RequestUri.Matches(other.RequestUri, UriPatternMatchingOptions))
        {
            return false;
        }

        if (HttpVersion is not null && other.Version != HttpVersion)
        {
            return false;
        }

        if (Content is not null)
        {
            if(other.Content is null)
            {
                return false;
            }

            var stringContent = other.Content.ReadAsStringAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            var contentMatches = Content switch
            {
                "" => stringContent == Content,
                "*" => true,
                _ => StringMatcher.Matches(stringContent, Content),
            };

            if (!contentMatches)
            {
                return false;
            }
        }

        return true;
    }
}
