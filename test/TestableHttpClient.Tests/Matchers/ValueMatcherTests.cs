using TestableHttpClient.Matchers;

namespace TestableHttpClient.Tests.Matchers;

public sealed class ValueMatcherTests
{
    private static bool MatchesExactValueCaseSensitive(string value, ExactValue exactValue) => ValueMatcher.Matches(value, exactValue, false);
    private static bool MatchesExactValueCaseInsensitive(string value, ExactValue exactValue) => ValueMatcher.Matches(value, exactValue, true);

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("Test")]
    public void MatchesExactValue_CaseSensitive_ReturnsTrue(string value)
    {
        Assert.True(MatchesExactValueCaseSensitive(value, new(value)));
    }

    [Theory]
    [InlineData("", "test")]
    [InlineData("test", "")]
    [InlineData("test", "Test")]
    [InlineData("Test", "Te")]
    [InlineData("Test", "st")]
    [InlineData("Test", "*st")]
    public void MatchesExactValue_CaseSensitive_ReturnswFalse(string value, string expectedValue)
    {
        Assert.False(MatchesExactValueCaseSensitive(value, new(expectedValue)));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("test", "test")]
    [InlineData("Test", "Test")]
    [InlineData("test", "Test")]
    [InlineData("Test", "test")]
    public void MatchesExactValue_CaseInsensitive_ReturnsTrue(string value, string expectedValue)
    {
        Assert.True(MatchesExactValueCaseInsensitive(value, new(expectedValue)));
    }

    [Theory]
    [InlineData("", "test")]
    [InlineData("test", "")]
    [InlineData("Test", "Te")]
    [InlineData("Test", "st")]
    [InlineData("Test", "*st")]
    public void MatchesExactValue_CaseInsensitive_ReturnswFalse(string value, string expectedValue)
    {
        Assert.False(MatchesExactValueCaseInsensitive(value, new(expectedValue)));
    }

    private static bool MatchesPatternValueCaseSensitive(string value, PatternValue pattern) => ValueMatcher.Matches(value, pattern, false);
    private static bool MatchesPatternValueCaseInsensitive(string value, PatternValue pattern) => ValueMatcher.Matches(value, pattern, true);

    public static TheoryData<string, string> CaseSensitivePatternValueHappyCases = new()
    {
        // Empty pattern
        { "", "" },
        { "", "*" },

        // Exact string matching
        { "test", "test" },
        { "Test", "Test" },
        { "t.st", "t.st" },
        { "t$st", "t$st" },

        // Wildcard patterns
        { "test", "*" },
        { "Test", "Te*" },
        { "testme", "te*" },
        { "test\n", "te*" },
        { "Test", "*st" },
        { "mytest", "*st" },
        { "Test", "T*st" },
        { "te\nst", "t*st" },
        { "Tst", "T*st" },
        { "Toast", "T*st" },
        { "tosti", "t*s*i" },
    };

    public static TheoryData<string, string> CaseInsensitivePatternValueHappyCases = new()
    {
        // Empty pattern
        { "", "" },
        { "", "*" },

        // Exact string matching
        { "test", "test" },
        { "Test", "Test" },
        { "Test", "test" },
        { "test", "Test" },
        { "t.st", "t.st" },
        { "T.st", "t.st" },
        { "t$st", "t$st" },

        // Wildcard patterns
        { "test", "*" },
        { "Test", "Te*" },
        { "Test", "te*" },
        { "testme", "te*" },
        { "test\n", "te*" },
        { "Test", "*st" },
        { "mytest", "*st" },
        { "Test", "T*st" },
        { "Tst", "T*st" },
        { "te\nst", "t*st" },
        { "Toast", "t*st" },
        { "Toast", "T*st" },
        { "tosti", "t*s*i" },
        { "tosti", "T*s*i" },
    };

    public static TheoryData<string, string> CaseSensitivePatternValueUnhappyCases = new()
    {
        // Empty pattern
        { "", "test" },
        { "test", "" },

        // Exact string matching
        { "Test", "test" },
        { "test", "Test" },
        { "mytest", "test" },
        { "testme", "test" },
        { "test", "mytest" },
        { "test", "testme" },
        { "test", "t.st" },
        { "test", "^test$" },
        { "test\n", "test" },

        // Wildcard patterns
        { "Tos", "T*st" },
        { "Rest", "Te*" },
        { "Tester", "*st" },
    };

    public static TheoryData<string, string> CaseInsensitivePatternValueUnhappyCases = new()
    {
        // Empty pattern
        { "", "test" },
        { "test", "" },

        // Exact string matching
        { "Toast", "test" },
        { "mytest", "test" },
        { "testme", "test" },
        { "test", "mytest" },
        { "test", "testme" },
        { "Test", "t.st" },
        { "test", "^test$" },
        { "test\n", "test" },

        // Wildcard patterns
        { "Tos", "T*st" },
        { "Rest", "Te*" },
        { "Tester", "*st" },
    };

    [Theory]
    [MemberData(nameof(CaseSensitivePatternValueHappyCases))]
    public void MatchesPatternValue_CaseSensitive_ReturnsTrue(string value, string pattern)
    {
        Assert.True(MatchesPatternValueCaseSensitive(value, new(pattern)));
    }

    [Theory]
    [MemberData(nameof(CaseSensitivePatternValueUnhappyCases))]
    public void MatchesPatternValue_CaseSensitive_ReturnsFalse(string value, string pattern)
    {
        Assert.False(MatchesPatternValueCaseSensitive(value, new(pattern)));
    }

    [Theory]
    [MemberData(nameof(CaseInsensitivePatternValueHappyCases))]
    public void MatchesPatternValue_CaseInsensitive_ReturnsTrue(string value, string pattern)
    {
        Assert.True(MatchesPatternValueCaseInsensitive(value, new(pattern)));
    }

    [Theory]
    [MemberData(nameof(CaseInsensitivePatternValueUnhappyCases))]
    public void MatchesPatternValue_CaseInsensitive_ReturnsFalse(string value, string pattern)
    {
        Assert.False(MatchesPatternValueCaseInsensitive(value, new(pattern)));
    }
}
