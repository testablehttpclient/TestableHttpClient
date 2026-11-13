using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient;

internal sealed record Request
{
    public Request(UriPatternMatchingOptions uriPatternMatchingOptions)
    {
        UriPatternMatchingOptions = uriPatternMatchingOptions;

        Method = new Any();
        RequestUri = new();
        Version = new Any();
        Headers = new Any();
        Content = new Any();
    }

    public UriPatternMatchingOptions UriPatternMatchingOptions { get; }

    public HttpMethodOrAny Method { get; set; }
    public UriPattern RequestUri { get; set; }
    public VersionOrAny Version { get; set; }

    public Headers Headers { get; set; }

    public Content Content { get; set; }

    public void AddHeader(string headerName) => AddHeader(headerName, new Any());

    public void AddHeader(string headerName, string patternValue) => AddHeader(headerName, new PatternValue(patternValue));

    public void AddHeader(string headerName, HeaderValue headerValue)
    {
        switch (Headers)
        {
            case Any:
                HeaderList newHeaderValues = new() { [headerName] = headerValue };
                Headers = new Headers(newHeaderValues);
                break;
            case HeaderList headerValues:
                headerValues[headerName] = headerValue;
                break;
            default:
                throw new UnreachableException();
        }
    }

    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "https://github.com/dotnet/sdk/issues/57085")]
    public bool Matches(HttpRequestMessage? other)
    {
        if (other is null)
        {
            return false;
        }

        bool methodMatches = Method switch
        {
            Any => true,
            HttpMethod value => other.Method == value,
        };

        if (!methodMatches)
        {
            return false;
        }

        if (other.RequestUri is not null && !RequestUri.Matches(other.RequestUri, UriPatternMatchingOptions))
        {
            return false;
        }

        bool versionMatches = Version switch
        {
            Any => true,
            Version value => other.Version == value,
        };

        if (!versionMatches)
        {
            return false;
        }

        if (Headers is HeaderList headerValues)
        {
            foreach (var keyValuePair in headerValues)
            {
                if (!other.Headers.HasHeader(keyValuePair.Key, keyValuePair.Value) && (other.Content is null || !other.Content.Headers.HasHeader(keyValuePair.Key, keyValuePair.Value)))
                {
                    return false;
                }
            }
        }

        string? stringContent = null;
        if (other.Content is not null)
        {
            stringContent = other.Content.ReadAsStringAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        bool contentMatches = Content switch
        {
            Any => true,
            PatternValue value when stringContent is null => false,
            PatternValue value => StringMatcher.Matches(stringContent, value.pattern, false),
        };

        if (!contentMatches)
        {
            return false;
        }
        return true;
    }
}
