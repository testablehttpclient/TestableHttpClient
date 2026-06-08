using System.Diagnostics;
using System.Globalization;

namespace TestableHttpClient.Utils;

internal static class MessageBuilder
{
    internal static string BuildMessage(int? expectedCount, int actualCount, Request expectedRequest)
    {
        var pass = expectedCount switch
        {
            null => actualCount > 0,
            _ => expectedCount == actualCount
        };
        var method = expectedRequest.Method.Value switch
        {
            Any => "",
            HttpMethod httpMethod => $"{httpMethod} ",
            _ => throw new UnreachableException()
        };

        var expectedMessage = expectedCount switch
        {
            null => $"at least one {method}request",
            0 => $"no {method}requests",
            1 => $"one {method}request",
            _ => $"{expectedCount} {method}requests"
        };

        var requestUri = BuildRequestUri(expectedRequest.RequestUri);

        var headers = expectedRequest.Headers.Value switch
        {
            AnyHeader => "",
            HeaderList headerValues => BuildHeaders(headerValues),
            _ => throw new UnreachableException()
        };

        string content = expectedRequest.Content.Value switch
        {
            AnyContent => "",
            Pattern pattern => BuildContent(pattern, string.IsNullOrEmpty(headers)),
            _ => throw new UnreachableException()
        };

        var actualMessage = actualCount switch
        {
            0 => "no requests were made",
            1 => "one request was made",
            _ => $"{actualCount} requests were made"
        };

        return pass switch
        {
            true => $"Expected {expectedMessage} to be made{requestUri}{headers}{content}, and {actualMessage}.",
            false => $"Expected {expectedMessage} to be made{requestUri}{headers}{content}, but {actualMessage}."
        };
    }

    private static string BuildRequestUri(UriPattern requestUri)
    {
        string scheme = BuildValue(requestUri.Scheme);
        string host = BuildValue(requestUri.Host);
        string port = BuildValue(requestUri.Port);
        string path = BuildValue(requestUri.Path);
        string query = BuildValue(requestUri.Query);

        StringBuilder uriBuilder = new();
        if (!string.IsNullOrEmpty(scheme))
        {
            uriBuilder.Append(scheme).Append("://");
        }

        if (!string.IsNullOrEmpty(host))
        {
            uriBuilder.Append(host);
        }
        else if (uriBuilder.Length > 0)
        {
            uriBuilder.Append("<any host>");
        }

        if (!string.IsNullOrEmpty(port))
        {
            uriBuilder.Append(':').Append(port);
        }

        if (!string.IsNullOrEmpty(path))
        {
            uriBuilder.Append(path);
        }

        if (!string.IsNullOrEmpty(query))
        {
            uriBuilder.Append('?').Append(query);
        }

        var result = uriBuilder.ToString();
        if (string.IsNullOrEmpty(result))
        {
            return result;
        }
        else
        {
            return $" to '{result}'";
        }
    }

    private static string BuildHeaders(HeaderList headerValues)
    {
        StringBuilder headers = new();

        foreach (var header in headerValues)
        {
            var value = BuildValue(header.Value);
            if (string.IsNullOrEmpty(value))
            {
                headers.AppendLine(CultureInfo.InvariantCulture, $"  {header.Key}: <any value>");
            }
            else
            {
                headers.AppendLine(CultureInfo.InvariantCulture, $"  {header.Key}: '{value}'");
            }
        }

        var result = headers.ToString();
        if (string.IsNullOrEmpty(result))
        {
            return result;
        }
        else
        {
            StringBuilder headervalue = new();
            headervalue.AppendLine(" with headers:").Append(result);
            return headervalue.ToString(); ;
        }
    }

    private static string BuildValue(Value value)
    {
        return value switch
        {
            AnyValue => "",
            ExactValue exactValue => exactValue.ExpectedValue,
            PatternValue patternValue => patternValue.ExpectedValue,
            _ => throw new UnreachableException()
        };
    }

    private static string BuildContent(Pattern value, bool hasHeaders)
    {
        string[] content = value.pattern.Split(['\n']);

        StringBuilder contentBuilder = new();
        if (hasHeaders)
        {
            contentBuilder.AppendLine(" with content:");
        }
        else
        {
            contentBuilder.AppendLine("and content:");
        }
        foreach (var line in content)
        {
            contentBuilder.AppendLine(CultureInfo.InvariantCulture, $"  {line.Trim('\r')}");
        }
        return contentBuilder.ToString();
    }
}
