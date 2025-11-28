using System.Globalization;

namespace TestableHttpClient.Utils;

internal static class HttpRequestMessageFormatter
{
    internal static string Format(HttpRequestMessage? request, HttpRequestMessageFormatOptions options)
    {
        if (request is null)
        {
            return "null";
        }

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        StringBuilder builder = new();
        if (options.HasFlag(HttpRequestMessageFormatOptions.RequestLine))
        {
            builder.Append(formatProvider, $"{request.Method} {request.RequestUri} HTTP/{request.Version}\r\n");
        }
        else
        {
            if (options.HasFlag(HttpRequestMessageFormatOptions.HttpMethod))
            {
                builder.Append(formatProvider, $"{request.Method}");
            }
            if (options.HasFlag(HttpRequestMessageFormatOptions.RequestUri))
            {
                if(builder.Length > 0)
                {
                    builder.Append(" ");
                }
                builder.Append(formatProvider, $"{request.RequestUri}");
            }
            if (options.HasFlag(HttpRequestMessageFormatOptions.HttpVersion))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" ");
                }
                builder.Append(formatProvider, $"HTTP/{request.Version}");
            }
            if(options.HasFlag(HttpRequestMessageFormatOptions.Headers) || options.HasFlag(HttpRequestMessageFormatOptions.Content))
            {
                builder.Append("\r\n");
            }
        }

        if (options.HasFlag(HttpRequestMessageFormatOptions.Headers))
        {
            foreach (var header in request.Headers)
            {
                builder.Append(formatProvider, $"{header.Key}: {string.Join(", ", header.Value)}\r\n");
            }
            foreach (var header in request.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
            {
                builder.Append(formatProvider, $"{header.Key}: {string.Join(", ", header.Value)}\r\n");
            }
        }

        if (options.HasFlag(HttpRequestMessageFormatOptions.Content))
        {
            builder.Append("\r\n");
            builder.Append(request.Content?.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        return builder.ToString();
    }
}

[Flags]
internal enum HttpRequestMessageFormatOptions
{
    HttpMethod = 1,
    RequestUri = 2,
    HttpVersion = 4,
    RequestLine = HttpMethod | RequestUri | HttpVersion,
    Headers = 8,
    Content = 16,
    All = RequestLine | Headers | Content
}
