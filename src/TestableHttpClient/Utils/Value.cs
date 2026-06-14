using System.Diagnostics;

namespace TestableHttpClient.Utils;

internal abstract record Value
{
    private static readonly Value _anyValue = new AnyValue();
    public static Value Any() => _anyValue;
    public static Value Exact(string value) => new ExactValue(value);
    public static Value Pattern(string pattern) => pattern == "*" ? _anyValue : new PatternValue(pattern);
    internal virtual bool Matches(string value, bool ignoreCase)
    {
        return this switch
        {
            AnyValue => true,
            ExactValue exactValue => exactValue.ExpectedValue.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal),
            PatternValue patternValue => StringMatcher.Matches(value, patternValue.ExpectedValue, ignoreCase),
            _ => false
        };
    }
}

[DebuggerDisplay("Any value")]
internal sealed record AnyValue : Value
{
}

[DebuggerDisplay("Exact value: {ExpectedValue}")]
internal sealed record ExactValue(string ExpectedValue) : Value
{
}

[DebuggerDisplay("Pattern value: {ExpectedValue}")]
internal sealed record PatternValue(string ExpectedValue) : Value
{
}
