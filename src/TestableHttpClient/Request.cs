using System.Diagnostics;

namespace TestableHttpClient;

internal readonly struct Any;
internal readonly union HttpMethodOrAny(Any, HttpMethod);
internal readonly union VersionOrAny(Any, Version);

internal sealed class HeaderList : Dictionary<string, UriPatternValue> { }
internal readonly union Headers(Any, HeaderList);

internal record struct Pattern(string pattern);
internal readonly union Content(Any, Pattern);

internal sealed record Request : IEquatable<HttpRequestMessage>
{
    public Request(UriPatternMatchingOptions uriPatternMatchingOptions)
    {
        UriPatternMatchingOptions = uriPatternMatchingOptions;

        Method = new Any();
        Version = new Any();
        Headers = new Any();
        Content = new Any();
    }

    public UriPatternMatchingOptions UriPatternMatchingOptions { get; }

    public HttpMethodOrAny Method { get; init; }
    public UriPattern RequestUri { get; init; } = new();
    public VersionOrAny Version { get; init; }

    public Headers Headers { get; init; }

    public Content Content { get; init; }

    public Request AddHeader(string headerName) => AddHeader(headerName, UriPatternValue.CreateAny());

    public Request AddHeader(string headerName, string headerValue) => AddHeader(headerName, UriPatternValue.CreatePattern(headerValue));

    public Request AddHeader(string headerName, UriPatternValue headerValue)
    {

        if (Headers is Any)
        {
            HeaderList headerValues = new() { [headerName] = headerValue };
            return this with { Headers = new Headers(headerValues) };
        }
        else if (Headers is HeaderList headerValues)
        {
            headerValues[headerName] = headerValue;
            return this;
        }
        else
        {
            throw new UnreachableException();
        }
    }

    public bool Equals(HttpRequestMessage? other)
    {
        if (other is null)
        {
            return false;
        }

        bool methodMatches = Method switch
        {
            Any => true,
            HttpMethod value => other.Method == value,
            _ => throw new UnreachableException()
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
            _ => throw new UnreachableException()
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
            Pattern value when stringContent is null => false,
            Pattern value => StringMatcher.Matches(stringContent, value.pattern, false),
            _ => throw new UnreachableException()
        };

        if (!contentMatches)
        {
            return false;
        }
        return true;
    }
}
