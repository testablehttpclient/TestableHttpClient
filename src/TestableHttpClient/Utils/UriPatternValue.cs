using System.Diagnostics;

namespace TestableHttpClient.Utils;

internal abstract record UriPatternValue
{
    private static readonly Any _anyValue = new();

    internal abstract bool Matches(string value, bool ignoreCase);

    [DebuggerDisplay("Any value")]
    internal sealed record Any : UriPatternValue
    {
        internal override bool Matches(string value, bool ignoreCase) => true;
    }

    [DebuggerDisplay("Exact value: {ExpectedValue}")]
    internal sealed record Exact(string ExpectedValue) : UriPatternValue
    {
        internal override bool Matches(string value, bool ignoreCase) =>
            ExpectedValue.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    [DebuggerDisplay("Pattern value: {ExpectedValue}")]
    internal sealed record Pattern(string ExpectedValue) : UriPatternValue
    {
        internal override bool Matches(string value, bool ignoreCase) =>
            StringMatcher.Matches(value, ExpectedValue, ignoreCase);
    }

    public static UriPatternValue CreateAny() => _anyValue;

    public static UriPatternValue CreateExact(string value) => new Exact(value);

    public static UriPatternValue CreatePattern(string pattern) => pattern == "*" ? _anyValue : new Pattern(pattern);
}
