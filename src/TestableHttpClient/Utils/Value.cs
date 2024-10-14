using System.Diagnostics;

namespace TestableHttpClient.Utils;

internal abstract record Value
{
    public static Value<string> Any() => Any<string>();
    public static Value<T> Any<T>() => new AnyValue<T>();
    public static Value<T> OneOf<T>(params T[] values) => new OneOfValue<T>(values);
    public static Value<string> Exact(string value) => Exact<string>(value);
    public static Value<T> Exact<T>(T value) => new ExactValue<T>(value);
    public static Value<string> Pattern(string pattern) => new PatternValue(pattern);

}

internal abstract record Value<T>
{
    public virtual bool IsAny => false;
    internal abstract bool Matches(T value, bool ignoreCase);
}

[DebuggerDisplay("Any value")]
file sealed record AnyValue<T> : Value<T>
{
    public override bool IsAny => true;
    internal override bool Matches(T value, bool ignoreCase) => true;
}

[DebuggerDisplay("Exact value: {expectedValue}")]
file sealed record ExactValue<T> : Value<T>
{
    private readonly T expectedValue;
    public ExactValue(T expectedValue) => this.expectedValue = expectedValue ?? throw new ArgumentNullException(nameof(expectedValue));
    internal override bool Matches(T value, bool ignoreCase)
    {
        if (expectedValue is string expectedStringValue && value is string stringValue)
        {
            return expectedStringValue.Equals(stringValue, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        return expectedValue!.Equals(value);
    }
}

file sealed record OneOfValue<T> : Value<T>
{
    private readonly IEnumerable<T> values;
    public OneOfValue(IEnumerable<T> values) => this.values = values;
    internal override bool Matches(T value, bool ignoreCase)
    {
        if (value is string stringValue)
        {
            return values.OfType<string>().Contains(stringValue, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        return values.Contains(value);
    }
}

[DebuggerDisplay("Pattern value: {pattern}")]
file sealed record PatternValue : Value<string>
{
    private readonly string pattern;
    public PatternValue(string pattern) => this.pattern = pattern;
    internal override bool Matches(string value, bool ignoreCase) => StringMatcher.Matches(value, pattern, ignoreCase);
}
