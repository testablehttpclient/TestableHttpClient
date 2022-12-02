using System.Diagnostics;

namespace TestableHttpClient.Utils;

internal static class UriPatternParser
{
    public static UriPattern Parse(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            throw new UriPatternParserException("An empty route isn't valid.");
        }

        return ParsePattern(pattern.AsSpan());
    }

    private static UriPattern ParsePattern(ReadOnlySpan<char> patternSpan)
    {
        int indexOfSchemeSeperator = Math.Max(0, patternSpan.IndexOf("://".AsSpan(), StringComparison.Ordinal));
        ReadOnlySpan<char> schemePattern = patternSpan[0..indexOfSchemeSeperator];
        patternSpan = patternSpan[indexOfSchemeSeperator..];
        int indexOfFragmentSeperator = patternSpan.LastIndexOf('#');
        if (indexOfFragmentSeperator == -1)
        {
            indexOfFragmentSeperator = patternSpan.Length;
        }

        _ = patternSpan[indexOfFragmentSeperator..];
        patternSpan = patternSpan[..indexOfFragmentSeperator];
        int indexOfQuerySeperator = patternSpan.LastIndexOf('?');
        if (indexOfQuerySeperator == -1)
        {
            indexOfQuerySeperator = patternSpan.Length;
        }

        ReadOnlySpan<char> queryPattern = patternSpan[indexOfQuerySeperator..];
        patternSpan = patternSpan[..indexOfQuerySeperator];

        if (patternSpan is ['/', '/', ..])
        {
            patternSpan = patternSpan[2..];
        }

        if (patternSpan is [':', '/', '/', ..])
        {
            patternSpan = patternSpan[3..];
        }

        int indexOfUserInfoSeperator = patternSpan.IndexOf('@');
        int indexOfPathSeperator = patternSpan.IndexOf('/');
        if (indexOfPathSeperator == -1)
        {
            indexOfPathSeperator = patternSpan.Length;
        }

        ReadOnlySpan<char> hostPattern = patternSpan[(indexOfUserInfoSeperator + 1)..indexOfPathSeperator];
        ReadOnlySpan<char> pathPattern = patternSpan[indexOfPathSeperator..];

        return new()
        {
            Scheme = ParseScheme(schemePattern),
            Host = ParseHost(hostPattern),
            Path = ParsePath(pathPattern),
            Query = ParseQuery(queryPattern)
        };

        static Value ParseScheme(ReadOnlySpan<char> scheme) => scheme switch
        {
            [] => Value.Any(),
            ['*'] => Value.Any(),
            _ when scheme.IndexOf('*') != -1 => Value.Pattern(scheme.ToString()),
            _ => Value.Exact(scheme.ToString())
        };
        static Value ParseHost(ReadOnlySpan<char> host) => host switch
        {
            [] => Value.Any(),
            ['*'] => Value.Any(),
            _ when host.IndexOf('*') != -1 => Value.Pattern(host.ToString()),
            _ => Value.Exact(host.ToString())
        };
        static Value ParsePath(ReadOnlySpan<char> path) => path switch
        {
            [] => Value.Any(),
            ['/', '*'] => Value.Any(),
            ['/', .. var rest] when rest.IndexOf('*') != -1 => Value.Pattern(path.ToString()),
            ['/', ..] => Value.Exact(path.ToString()),
            _ => throw new UnreachableException()
        };
        static Value ParseQuery(ReadOnlySpan<char> query) => query switch
        {
            [] => Value.Any(),
            ['?'] => Value.Any(),
            ['?', '*'] => Value.Any(),
            ['?', .. var rest] when rest.IndexOf('*') != -1 => Value.Pattern(rest.ToString()),
            ['?', .. var rest] => Value.Exact(rest.ToString()),
            _ => throw new UnreachableException()
        };
    }
}
