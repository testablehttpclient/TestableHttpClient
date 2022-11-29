namespace TestableHttpClient.Utils;

internal static class UriPatternParser
{
    public static UriPattern Parse(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            throw new UriPatternParserException("An empty route isn't valid.");
        }

        ReadOnlySpan<char> patternSpan = pattern.AsSpan();

        if (patternSpan is ['*'])
        {
            return UriPattern.Any;
        }
        else
        {
            int currentPosition = 0;
            Value scheme = ParseScheme(patternSpan, ref currentPosition);
            patternSpan = patternSpan[currentPosition..];
            currentPosition = 0;
            Value host = ParseHost(patternSpan, ref currentPosition);
            patternSpan = patternSpan[currentPosition..];
            currentPosition = 0;

            Value path = ParsePath(patternSpan, ref currentPosition);
            patternSpan = patternSpan[currentPosition..];
            currentPosition = 0;

            Value query = ParseQuery(patternSpan);

            return new UriPattern
            {
                Scheme = scheme,
                Host = host,
                Path = path,
                Query = query,
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
            throw new UriPatternParserException("No scheme specified, this is not a valid url.");
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
        int indexOfWildCard = patternSpan.IndexOf('*');

        int indexOfPathSeparator = patternSpan.IndexOf('/');
        int indexOfQuerySeparator = patternSpan.IndexOf('?');

        if (indexOfQuerySeparator > -1 && (indexOfPathSeparator == -1 || indexOfQuerySeparator < indexOfPathSeparator))
        {
            throw new UriPatternParserException("invalid queryParameter.");
        }

        if (indexOfPathSeparator <= -1)
        {
            indexOfPathSeparator = patternSpan.Length;
        }

        if (indexOfPathSeparator == currentPosition)
        {
            return Value.Any();
        }

        if (indexOfWildCard == currentPosition && indexOfPathSeparator == currentPosition + 1)
        {
            currentPosition++;
            return Value.Any();
        }

        bool hasWildCard = -1 < indexOfWildCard && indexOfWildCard < indexOfPathSeparator;
        string value = patternSpan[currentPosition..indexOfPathSeparator].ToString();
        currentPosition = indexOfPathSeparator;

        return hasWildCard switch
        {
            true => Value.Pattern(value),
            false => Value.Exact(value)
        };
    }

    private static Value ParsePath(ReadOnlySpan<char> patternSpan, ref int currentPosition)
    {
        int indexOfQuestionMark = patternSpan.IndexOf('?');
        if (indexOfQuestionMark == 0)
        {
            throw new UriPatternParserException("Unexpected begin of query pattern.");
        }
        if (indexOfQuestionMark == -1)
        {
            indexOfQuestionMark = patternSpan.Length;
        }
        currentPosition = indexOfQuestionMark;
        var pathSpan = patternSpan[..indexOfQuestionMark];
        return pathSpan switch
        {
            [] => Value.Any(),
            ['/'] => Value.Exact(pathSpan.ToString()),
            ['/', '*'] => Value.Any(),
            ['/', ..] when pathSpan.IndexOf('*') > 0 => Value.Pattern(pathSpan.ToString()),
            ['/', ..] => Value.Exact(pathSpan.ToString()),
            _ => throw new System.Diagnostics.UnreachableException()
        };
    }

    private static Value ParseQuery(ReadOnlySpan<char> patternSpan)
    {
        return patternSpan switch
        {
            [] => Value.Any(),
            ['?'] => throw new UriPatternParserException("There are no query parameters."),
            ['?', '*'] => Value.Any(),
            ['?', ..] when patternSpan.IndexOf('*') > 0 => Value.Pattern(patternSpan.ToString()),
            ['?', ..] => Value.Exact(patternSpan.ToString()),
            _ => throw new System.Diagnostics.UnreachableException()
        };
    }
}
