using System.Diagnostics;

namespace TestableHttpClient.Utils;

internal abstract record Value
{
    private static readonly Value _anyValue = new AnyValue();
    public static Value Any() => _anyValue;
    public static Value Exact(string value) => new ExactValue(value);
    public static Value Pattern(string pattern) => pattern == "*" ? _anyValue : new PatternValue(pattern);
    internal abstract bool Matches(string value, bool ignoreCase);
}

[DebuggerDisplay("Any value")]
internal sealed record AnyValue : Value
{
    internal override bool Matches(string value, bool ignoreCase) => true;
}

[DebuggerDisplay("Exact value: {ExpectedValue}")]
internal sealed record ExactValue(string ExpectedValue) : Value
{
    internal override bool Matches(string value, bool ignoreCase) => ExpectedValue.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
}

[DebuggerDisplay("Pattern value: {ExpectedValue}")]
internal sealed record PatternValue(string ExpectedValue) : Value
{
    internal override bool Matches(string value, bool ignoreCase) => StringMatcher.Matches(value, ExpectedValue, ignoreCase);
}
