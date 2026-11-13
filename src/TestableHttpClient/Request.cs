using System.Diagnostics;

namespace TestableHttpClient;

internal sealed record Request
{
    public Request()
    {
        Method = new Any();
        RequestUri = new();
        Version = new Any();
        Headers = new Any();
        Content = new Any();
    }

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
}
