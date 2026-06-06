using System.Diagnostics;

namespace TestableHttpClient;

internal record struct AnyHeader;
internal sealed class HeaderList : Dictionary<string, Value> { }
internal readonly struct Headers
{
    public readonly object? Value { get; }
    public Headers() => Value = new AnyHeader();
    public Headers(AnyHeader value) => Value = value;
    public Headers(HeaderList value) => Value = value;
}

internal sealed record Request : IEquatable<HttpRequestMessage>
{
    public Request(UriPatternMatchingOptions uriPatternMatchingOptions)
    {
        UriPatternMatchingOptions = uriPatternMatchingOptions;
    }

    public UriPatternMatchingOptions UriPatternMatchingOptions { get; }

    public HttpMethod? Method { get; init; }
    public UriPattern? RequestUri { get; init; }
    public Version? Version { get; init; }

    public Headers Headers { get; init; } = new();

    public string? Content { get; init; }

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

        if (Method is not null && other.Method != Method)
        {
            return false;
        }

        if (RequestUri is not null && other.RequestUri is not null && !RequestUri.Matches(other.RequestUri, UriPatternMatchingOptions))
        {
            return false;
        }

        if (Version is not null && other.Version != Version)
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

        if (Content is not null)
        {
            if (other.Content is null)
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
