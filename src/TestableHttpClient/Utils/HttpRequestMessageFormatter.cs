using System.Globalization;

namespace TestableHttpClient.Utils;

internal static class HttpRequestMessageFormatter
{
    internal static string Format(HttpRequestMessage? request, RequestFormatOptions options)
    {
        if (request is null)
        {
            return "null";
        }

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        StringBuilder builder = new();
        if (options.HasFlag(RequestFormatOptions.RequestLine))
        {
            builder.Append(formatProvider, $"{request.Method} {request.RequestUri} HTTP/{request.Version}\r\n");
        }
        else
        {
            if (options.HasFlag(RequestFormatOptions.HttpMethod))
            {
                builder.Append(formatProvider, $"{request.Method}");
            }
            if (options.HasFlag(RequestFormatOptions.RequestUri))
            {
                if(builder.Length > 0)
                {
                    builder.Append(" ");
                }
                builder.Append(formatProvider, $"{request.RequestUri}");
            }
            if (options.HasFlag(RequestFormatOptions.HttpVersion))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" ");
                }
                builder.Append(formatProvider, $"HTTP/{request.Version}");
            }
            if(options.HasFlag(RequestFormatOptions.Headers) || options.HasFlag(RequestFormatOptions.Content))
            {
                builder.Append("\r\n");
            }
        }

        if (options.HasFlag(RequestFormatOptions.Headers))
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

        if (options.HasFlag(RequestFormatOptions.Content))
        {
            builder.Append("\r\n");
            builder.Append(request.Content?.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        return builder.ToString();
    }
}
