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
        int length = patternSpan.Length;
        int currentPosition = 0;
        char currentChar = patternSpan[currentPosition];

        if (currentChar == '*' && length == 1)
        {
            return routeDefinition;
        }
        else
        {
            Value scheme = ParseScheme(patternSpan, ref currentPosition, length);
            Value host = ParseHost(patternSpan, ref currentPosition, length);
            Value path = ParsePath(patternSpan, ref currentPosition, length);
            return new RouteDefinition
            {
                Scheme = scheme,
                Host = host,
                Path = path
            };
        }
    }

    private static Value ParseScheme(ReadOnlySpan<char> patternSpan, ref int currentPosition, int length)
    {
        ReadOnlySpan<char> separator = "://".AsSpan();
        int beginPosition = currentPosition;
        char currentChar = patternSpan[currentPosition];
        char nextChar = patternSpan[currentPosition + 1];

        if (!patternSpan.Contains(separator, StringComparison.Ordinal))
        {
            return Value.Any();
        }

        if (currentChar == '*' && (nextChar == ':' || nextChar == '/'))
        {
            if (patternSpan[(currentPosition + 1)..(currentPosition + 4)].SequenceEqual(separator))
            {
                currentPosition += 4;
            }
            return Value.Any();
        }

        bool hasWildCard = false;

        while (currentChar != ':' && currentPosition < length)
        {
            hasWildCard = hasWildCard || currentChar == '*';
            currentChar = patternSpan[++currentPosition];
        }

        string value = patternSpan[beginPosition..currentPosition].ToString();

        if (patternSpan[currentPosition..(currentPosition + 3)].StartsWith(separator, StringComparison.Ordinal))
        {
            currentPosition += 3;
        }

        return hasWildCard switch
        {
            true => Value.Pattern(value),
            false => Value.Exact(value)
        };
    }

    private static Value ParseHost(ReadOnlySpan<char> patternSpan, ref int currentPosition, int length)
    {
        char currentChar = patternSpan[currentPosition];

        if (currentChar == '/')
        {
            return Value.Any();
        }

        if (currentChar == '*' && (currentPosition + 1 == length || patternSpan[currentPosition + 1] == '/'))
        {
            if (currentPosition + 1 != length)
            {
                currentPosition++;
            }
            return Value.Any();
        }
        int beginPosition = currentPosition;
        bool hasWildCard = false;

        while (currentChar != '/' && currentPosition < length)
        {
            hasWildCard = hasWildCard || currentChar == '*';
            currentPosition++;
            if (currentPosition < length)
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
    private static Value ParsePath(ReadOnlySpan<char> patternSpan, ref int currentPosition, int length)
    {
        if (currentPosition == length)
        {
            return Value.Any();
        }

        var pathSpan = patternSpan[currentPosition..length];

        return (pathSpan[0] == '/', pathSpan.IndexOf('*')) switch
        {
            (true, -1) => Value.Exact(pathSpan.ToString()),
            (true, 1) => Value.Any(),
            (true, _) => Value.Pattern(pathSpan.ToString()),
            (false, _) => Value.Any()
        };
    }
}
