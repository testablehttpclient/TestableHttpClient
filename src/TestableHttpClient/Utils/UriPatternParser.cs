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
        int indexOfSchemeSeparator = Math.Max(0, patternSpan.IndexOf("://".AsSpan(), StringComparison.Ordinal));
        ReadOnlySpan<char> schemePattern = patternSpan[0..indexOfSchemeSeparator];
        patternSpan = patternSpan[indexOfSchemeSeparator..];
        int indexOfFragmentSeparator = patternSpan.LastIndexOf('#');
        if (indexOfFragmentSeparator == -1)
        {
            indexOfFragmentSeparator = patternSpan.Length;
        }

        _ = patternSpan[indexOfFragmentSeparator..];
        patternSpan = patternSpan[..indexOfFragmentSeparator];
        int indexOfQuerySeparator = patternSpan.LastIndexOf('?');
        if (indexOfQuerySeparator == -1)
        {
            indexOfQuerySeparator = patternSpan.Length;
        }

        ReadOnlySpan<char> queryPattern = patternSpan[indexOfQuerySeparator..];
        patternSpan = patternSpan[..indexOfQuerySeparator];

        if (patternSpan is ['/', '/', ..])
        {
            patternSpan = patternSpan[2..];
        }

        if (patternSpan is [':', '/', '/', ..])
        {
            patternSpan = patternSpan[3..];
        }

        int indexOfUserInfoSeparator = patternSpan.IndexOf('@');
        patternSpan = patternSpan[(indexOfUserInfoSeparator + 1)..];

        int indexOfPathSeparator = patternSpan.IndexOf('/');
        if (indexOfPathSeparator == -1)
        {
            indexOfPathSeparator = patternSpan.Length;
        }

        ReadOnlySpan<char> hostPattern = patternSpan[..indexOfPathSeparator];
        ReadOnlySpan<char> pathPattern = patternSpan[indexOfPathSeparator..];
        int indexOfPortSeparator = hostPattern.LastIndexOf(':');
        int indexOfIpV6Part = hostPattern.LastIndexOf(']');
        if (indexOfPortSeparator == -1 || indexOfPortSeparator < indexOfIpV6Part)
        {
            indexOfPortSeparator = hostPattern.Length;
        }
        ReadOnlySpan<char> portPattern = hostPattern[indexOfPortSeparator..];
        hostPattern = hostPattern[..indexOfPortSeparator];

        return new()
        {
            Scheme = ParseScheme(schemePattern),
            Host = ParseHost(hostPattern),
            Port = ParsePort(portPattern),
            Path = ParsePath(pathPattern),
            Query = ParseQuery(queryPattern)
        };

        static UriPatternValue ParseScheme(ReadOnlySpan<char> scheme)
        {
            return scheme switch
            {
                [] => new Any(),
                ['*'] => new Any(),
                _ when scheme.IndexOf('*') != -1 => new PatternValue(scheme.ToString()),
                _ => new ExactValue(scheme.ToString())
            };
        }

        static UriPatternValue ParseHost(ReadOnlySpan<char> host)
        {
            return host switch
            {
                [] => new Any(),
                ['*'] => new Any(),
                _ when host.IndexOf('*') != -1 => new PatternValue(host.ToString()),
                _ => new ExactValue(host.ToString())
            };
        }

        static UriPatternValue ParsePort(ReadOnlySpan<char> port)
        {
            return port switch
            {
                [] => new Any(),
                [':'] => throw new UriPatternParserException("Invalid port"),
                [':', '*'] => new Any(),
                [':', .. var rest] when rest.IndexOf('*') != -1 => new PatternValue(rest.ToString()),
                [':', .. var rest] => new ExactValue(rest.ToString()),
                _ => throw new UnreachableException()
            };
        }

        static UriPatternValue ParsePath(ReadOnlySpan<char> path)
        {
            return path switch
            {
                [] => new Any(),
                ['/', '*'] => new Any(),
                ['/', .. var rest] when rest.IndexOf('*') != -1 => new PatternValue(path.ToString()),
                ['/', ..] => new ExactValue(path.ToString()),
                _ => throw new UnreachableException()
            };
        }

        static UriPatternValue ParseQuery(ReadOnlySpan<char> query)
        {
            return query switch
            {
                [] => new Any(),
                ['?'] => new Any(),
                ['?', '*'] => new Any(),
                ['?', .. var rest] when rest.IndexOf('*') != -1 => new PatternValue(rest.ToString()),
                ['?', .. var rest] => new ExactValue(rest.ToString()),
                _ => throw new UnreachableException()
            };
        }
    }
}
