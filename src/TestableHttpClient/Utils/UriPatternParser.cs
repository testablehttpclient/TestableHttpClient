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
        patternSpan = patternSpan[(indexOfUserInfoSeperator + 1)..];

        int indexOfPathSeperator = patternSpan.IndexOf('/');
        if (indexOfPathSeperator == -1)
        {
            indexOfPathSeperator = patternSpan.Length;
        }

        ReadOnlySpan<char> hostPattern = patternSpan[..indexOfPathSeperator];
        ReadOnlySpan<char> pathPattern = patternSpan[indexOfPathSeperator..];
        int indexOfPortSeperator = hostPattern.LastIndexOf(':');
        int indexOfIpV6Part = hostPattern.LastIndexOf(']');
        if (indexOfPortSeperator == -1 || indexOfPortSeperator < indexOfIpV6Part)
        {
            indexOfPortSeperator = hostPattern.Length;
        }
        ReadOnlySpan<char> portPattern = hostPattern[indexOfPortSeperator..];
        hostPattern = hostPattern[..indexOfPortSeperator];

        return new()
        {
            Scheme = ParseScheme(schemePattern),
            Host = ParseHost(hostPattern),
            Port = ParsePort(portPattern),
            Path = ParsePath(pathPattern),
            Query = ParseQuery(queryPattern)
        };

        static Value<string> ParseScheme(ReadOnlySpan<char> scheme) => scheme switch
        {
            [] => Value.Any(),
            ['*'] => Value.Any(),
            _ when scheme.IndexOf('*') != -1 => Value.Pattern(scheme.ToString()),
            _ => Value.Exact(scheme.ToString())
        };

        static Value<string> ParseHost(ReadOnlySpan<char> host) => host switch
        {
            [] => Value.Any(),
            ['*'] => Value.Any(),
            _ when host.IndexOf('*') != -1 => Value.Pattern(host.ToString()),
            _ => Value.Exact(host.ToString())
        };

        static Value<string> ParsePort(ReadOnlySpan<char> port) => port switch
        {
            [] => Value.Any(),
            [':'] => throw new UriPatternParserException("Invalid port"),
            [':', '*'] => Value.Any(),
            [':', .. var rest] when rest.IndexOf('*') != -1 => Value.Pattern(rest.ToString()),
            [':', .. var rest] => Value.Exact(rest.ToString()),
            _ => throw new UnreachableException()
        };

        static Value<string> ParsePath(ReadOnlySpan<char> path) => path switch
        {
            [] => Value.Any(),
            ['/', '*'] => Value.Any(),
            ['/', .. var rest] when rest.IndexOf('*') != -1 => Value.Pattern(path.ToString()),
            ['/', ..] => Value.Exact(path.ToString()),
            _ => throw new UnreachableException()
        };

        static Value<string> ParseQuery(ReadOnlySpan<char> query) => query switch
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
