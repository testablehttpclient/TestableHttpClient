using System.Diagnostics;

namespace TestableHttpClient.Utils;

internal abstract record Value
{
    private static readonly Value _anyValue = new AnyValue();
    public static Value Any() => _anyValue;
    public static Value Exact(string value) => new ExactValue(value);
    public static Value Pattern(string pattern) => new PatternValue(pattern);
    internal abstract bool Matches(string value, bool ignoreCase);
}

[DebuggerDisplay("Any value")]
file sealed record AnyValue : Value
{
    internal override bool Matches(string value, bool ignoreCase) => true;
}

[DebuggerDisplay("Exact value: {expectedValue}")]
file sealed record ExactValue : Value
{
    private readonly string expectedValue;
    public ExactValue(string expectedValue) => this.expectedValue = expectedValue;
    internal override bool Matches(string value, bool ignoreCase) => expectedValue.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
}

[DebuggerDisplay("Pattern value: {pattern}")]
file sealed record PatternValue : Value
{
    private readonly string pattern;
    public PatternValue(string pattern) => this.pattern = pattern;
    internal override bool Matches(string value, bool ignoreCase) => StringMatcher.Matches(value, pattern, ignoreCase);
}
