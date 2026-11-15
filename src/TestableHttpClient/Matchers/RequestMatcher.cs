using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient.Matchers;

internal static class RequestMatcher
{
    [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "https://github.com/dotnet/sdk/issues/57085")]
    public static bool Matches(Request expected, HttpRequestMessage actual, UriPatternMatchingOptions options)
    {
        if (actual is null)
        {
            return false;
        }

        bool methodMatches = expected.Method switch
        {
            Any => true,
            HttpMethod value => actual.Method == value,
        };

        if (!methodMatches)
        {
            return false;
        }

        if (actual.RequestUri is not null && !ValueMatcher.Matches(expected.RequestUri, actual.RequestUri, options))
        {
            return false;
        }

        bool versionMatches = expected.Version switch
        {
            Any => true,
            Version value => actual.Version == value,
        };

        if (!versionMatches)
        {
            return false;
        }

        if (expected.Headers is HeaderList headerValues)
        {
            foreach (var keyValuePair in headerValues)
            {
                if (!actual.Headers.HasHeader(keyValuePair.Key, keyValuePair.Value) && (actual.Content is null || !actual.Content.Headers.HasHeader(keyValuePair.Key, keyValuePair.Value)))
                {
                    return false;
                }
            }
        }

        string? stringContent = null;
        if (actual.Content is not null)
        {
            stringContent = actual.Content.ReadAsStringAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        bool contentMatches = expected.Content switch
        {
            Any => true,
            PatternValue value when stringContent is null => false,
            PatternValue value => ValueMatcher.Matches(value, stringContent, false),
        };

        if (!contentMatches)
        {
            return false;
        }
        return true;
    }

    internal static bool HasHeader(this HttpHeaders headers, string headerName, HeaderValue headerValue)
    {
        if (!headers.TryGetValues(headerName, out var values))
        {
            return false;
        }

        var value = string.Join(" ", values);
        return headerValue switch
        {
            Any => true,
            PatternValue pattern => ValueMatcher.Matches(pattern, value, false),
        };
    }
}
