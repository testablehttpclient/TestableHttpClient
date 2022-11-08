namespace TestableHttpClient.Utils;

internal abstract class Value
{
    private static readonly Value _anyValue = new AnyValue();
    public static Value Any() => _anyValue;
    public static Value Exact(string value) => new ExactValue(value);
    public static Value Pattern(string pattern) => new PatternValue(pattern);
    internal abstract bool Matches(string value, bool ignoreCase);
}

file class AnyValue : Value
{
    internal override bool Matches(string value, bool ignoreCase) => true;
}

file class ExactValue : Value
{
    private readonly string expectedValue;
    public ExactValue(string expectedValue) => this.expectedValue = expectedValue;
    internal override bool Matches(string value, bool ignoreCase) => expectedValue.Equals(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
}

file class PatternValue : Value
{
    private readonly string pattern;
    public PatternValue(string pattern) => this.pattern = pattern;
    internal override bool Matches(string value, bool ignoreCase) => StringMatcher.Matches(value, pattern, ignoreCase);
}
