using System.IO;

namespace TestableHttpClient.Utils;

internal static class RouteParser
{
    public static RouteDefinition Parse(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            throw new RouteParserException();
        }

        ReadOnlySpan<char> patternSpan = pattern.AsSpan();
        RouteDefinition routeDefinition = RouteDefinition.Any;
        int currentPosition = 0;
        char currentChar = patternSpan[currentPosition];

        if (currentChar == '*' && patternSpan.Length == 1)
        {
            return routeDefinition;
        }
        else
        {
            Value scheme = ParseScheme(patternSpan, ref currentPosition);
            Value host = ParseHost(patternSpan, ref currentPosition);
            Value path = ParsePath(patternSpan, ref currentPosition);
            return new RouteDefinition
            {
                Scheme = scheme,
                Host = host,
                Path = path
            };
        }
    }

    private static Value ParseScheme(ReadOnlySpan<char> patternSpan, ref int currentPosition)
    {
        ReadOnlySpan<char> separator = "://".AsSpan();
        int separatorIndex = patternSpan.IndexOf(separator, StringComparison.Ordinal);

        if (separatorIndex == -1)
        {
            return Value.Any();
        }

        if (separatorIndex == 0)
        {
            throw new RouteParserException("No scheme specified, this is not a valid url.");
        }

        int indexOfWildcard = patternSpan.IndexOf('*');
        bool hasWildCard = -1 < indexOfWildcard && indexOfWildcard < separatorIndex;

        int beginPosition = currentPosition;
        currentPosition = separatorIndex + 3;
        string value = patternSpan[beginPosition..separatorIndex].ToString();

        return hasWildCard switch
        {
            true when separatorIndex == 1 => Value.Any(),
            true => Value.Pattern(value),
            false => Value.Exact(value)
        };
    }

    private static Value ParseHost(ReadOnlySpan<char> patternSpan, ref int currentPosition)
    {
        char currentChar = patternSpan[currentPosition];

        if (currentChar == '/')
        {
            return Value.Any();
        }

        if (currentChar == '*' && (currentPosition + 1 == patternSpan.Length || patternSpan[currentPosition + 1] == '/'))
        {
            if (currentPosition + 1 != patternSpan.Length)
            {
                currentPosition++;
            }
            return Value.Any();
        }
        int beginPosition = currentPosition;
        bool hasWildCard = false;

        while (currentChar != '/' && currentPosition < patternSpan.Length)
        {
            hasWildCard = hasWildCard || currentChar == '*';
            currentPosition++;
            if (currentPosition < patternSpan.Length)
            {
                currentChar = patternSpan[currentPosition];
            }
        }

        string value = patternSpan[beginPosition..currentPosition].ToString();

        return hasWildCard switch
        {
            true => Value.Pattern(value),
            false => Value.Exact(value)
        };
    }

    private static Value ParsePath(ReadOnlySpan<char> patternSpan, ref int currentPosition)
    {
        ReadOnlySpan<char> pathSpan = currentPosition == patternSpan.Length ? ReadOnlySpan<char>.Empty : patternSpan[currentPosition..];

        return pathSpan switch
        {
            [] => Value.Any(),
            ['*'] => Value.Any(),
            ['/'] => Value.Exact(pathSpan.ToString()),
            ['/', '*'] => Value.Any(),
            ['/', ..] when pathSpan.IndexOf('*') > 0 => Value.Pattern(pathSpan.ToString()),
            ['/', ..] => Value.Exact(pathSpan.ToString()),
#if NET7_0_OR_GREATER
            _ => throw new System.Diagnostics.UnreachableException()
#else
            _ => throw new InvalidOperationException()
#endif
        };
    }
}
