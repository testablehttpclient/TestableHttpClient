using System.Diagnostics;

namespace TestableHttpClient;

internal record struct Any;
internal readonly struct AnyOr<T>
{
    public readonly object? Value { get; }
    public AnyOr() => Value = new Any();
    public AnyOr(Any value) => Value = value;
    public AnyOr(T value) => Value = value;
}

internal record struct AnyHeader;
internal sealed class HeaderList : Dictionary<string, Value> { }
internal readonly struct Headers
{
    public readonly object? Value { get; }
    public Headers() => Value = new AnyHeader();
    public Headers(AnyHeader value) => Value = value;
    public Headers(HeaderList value) => Value = value;
}

internal record struct AnyContent;
internal record struct Pattern(string pattern);
internal readonly struct Content
{
    public readonly object? Value { get; }
    public Content() => Value = new AnyContent();
    public Content(AnyContent value) => Value = value;
    public Content(Pattern value) => Value = value;
}

internal sealed record Request : IEquatable<HttpRequestMessage>
{
    public Request(UriPatternMatchingOptions uriPatternMatchingOptions)
    {
        UriPatternMatchingOptions = uriPatternMatchingOptions;
    }

    public UriPatternMatchingOptions UriPatternMatchingOptions { get; }

    public AnyOr<HttpMethod> Method { get; init; } = new();
    public UriPattern RequestUri { get; init; } = new();
    public AnyOr<Version> Version { get; init; } = new();

    public Headers Headers { get; init; } = new();

    public Content Content { get; init; } = new();

    public Request AddHeader(string headerName) => AddHeader(headerName, Value.Any());

    public Request AddHeader(string headerName, string headerValue) => AddHeader(headerName, Value.Pattern(headerValue));

    public Request AddHeader(string headerName, Value headerValue)
    {

        if (Headers.Value is AnyHeader)
        {
            HeaderList headerValues = new() { [headerName] = headerValue };
            return this with { Headers = new Headers(headerValues) };
        }
        else if (Headers.Value is HeaderList headerValues)
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

        bool methodMatches = Method.Value switch
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

        bool versionMatches = Version.Value switch
        {
            Any => true,
            Version value => other.Version == value,
            _ => throw new UnreachableException()
        };

        if (!versionMatches)
        {
            return false;
        }

        if (Headers.Value is HeaderList headerValues)
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

        bool contentMatches = Content.Value switch
        {
            AnyContent => true,
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
