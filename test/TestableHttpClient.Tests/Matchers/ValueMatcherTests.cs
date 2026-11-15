using TestableHttpClient.Matchers;

namespace TestableHttpClient.Tests.Matchers;

public sealed class ValueMatcherTests
{
    private static bool MatchesExactValueCaseSensitive(ExactValue expected, string actual) => ValueMatcher.Matches(expected, actual, false);
    private static bool MatchesExactValueCaseInsensitive(ExactValue expected, string actual) => ValueMatcher.Matches(expected, actual, true);

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("Test")]
    public void MatchesExactValue_CaseSensitive_ReturnsTrue(string value)
    {
        Assert.True(MatchesExactValueCaseSensitive(new(value), value));
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
        Assert.False(MatchesExactValueCaseSensitive(new(expectedValue), value));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("test", "test")]
    [InlineData("Test", "Test")]
    [InlineData("test", "Test")]
    [InlineData("Test", "test")]
    public void MatchesExactValue_CaseInsensitive_ReturnsTrue(string value, string expectedValue)
    {
        Assert.True(MatchesExactValueCaseInsensitive(new(expectedValue), value));
    }

    [Theory]
    [InlineData("", "test")]
    [InlineData("test", "")]
    [InlineData("Test", "Te")]
    [InlineData("Test", "st")]
    [InlineData("Test", "*st")]
    public void MatchesExactValue_CaseInsensitive_ReturnswFalse(string value, string expectedValue)
    {
        Assert.False(MatchesExactValueCaseInsensitive(new(expectedValue), value));
    }

    private static bool MatchesPatternValueCaseSensitive(PatternValue expected, string actual) => ValueMatcher.Matches(expected, actual, false);
    private static bool MatchesPatternValueCaseInsensitive(PatternValue expected, string actual) => ValueMatcher.Matches(expected, actual, true);

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
        Assert.True(MatchesPatternValueCaseSensitive(new(pattern), value));
    }

    [Theory]
    [MemberData(nameof(CaseSensitivePatternValueUnhappyCases))]
    public void MatchesPatternValue_CaseSensitive_ReturnsFalse(string value, string pattern)
    {
        Assert.False(MatchesPatternValueCaseSensitive(new(pattern), value));
    }

    [Theory]
    [MemberData(nameof(CaseInsensitivePatternValueHappyCases))]
    public void MatchesPatternValue_CaseInsensitive_ReturnsTrue(string value, string pattern)
    {
        Assert.True(MatchesPatternValueCaseInsensitive(new(pattern), value));
    }

    [Theory]
    [MemberData(nameof(CaseInsensitivePatternValueUnhappyCases))]
    public void MatchesPatternValue_CaseInsensitive_ReturnsFalse(string value, string pattern)
    {
        Assert.False(MatchesPatternValueCaseInsensitive(new(pattern), value));
    }

    private readonly UriPatternMatchingOptions defaultOptions = new();

    [Theory]
    [InlineData("https://httpbin.com/get")]
    [InlineData("http://httpbin.com/post")]
    [InlineData("https://httpbin.com/get?test=test")]
    [InlineData("https://httpbin.com:5000/get?test=test")]
    public void Matches_AnyUriPattern_MatchesAllUrls(string uriString)
    {
        UriPattern pattern = new();

        bool result = ValueMatcher.Matches(pattern, new Uri(uriString), defaultOptions);

        Assert.True(result);
    }

    [Theory]
    [InlineData("https")]
    [InlineData("HTTPS")]
    public void Matches_SpecificScheme_MatchesAnyUrlWithThatProtocol(string expectedScheme)
    {
        UriPattern pattern = new() { Scheme = new ExactValue(expectedScheme) };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("http://httpbin.com/get"), defaultOptions));
    }

    [Theory]
    [InlineData("http*")]
    [InlineData("HTTP*")]
    public void Matches_PatternScheme_Matches_AnyUrlMatchingTheSchemePattern(string expectedSchemePattern)
    {
        UriPattern pattern = new() { Scheme = new PatternValue(expectedSchemePattern) };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("http://httpbin.com/get"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("ftp://httpbin.com/get"), defaultOptions));
    }

    [Theory]
    [InlineData("httpbin.com")]
    [InlineData("HTTPBIN.COM")]
    public void Matches_SpecificHost_MatchesAnyUrlWithThatHost(string expectedHost)
    {
        UriPattern pattern = new() { Host = new ExactValue(expectedHost) };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.org/get"), defaultOptions));
    }

    [Theory]
    [InlineData("httpbin.*")]
    [InlineData("HTTPBIN.*")]
    public void Matches_PatternHost_MatchesAnyUrlWithThatHost(string expectedHostPattern)
    {
        UriPattern pattern = new() { Host = new PatternValue(expectedHostPattern) };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.org/get"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin/get"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactPort_MatchesAnyUrlWithThatPort()
    {
        UriPattern uriPattern = new() { Port = new ExactValue("80") };

        Assert.True(ValueMatcher.Matches(uriPattern, new Uri("http://httpbin.com"), defaultOptions));
        Assert.True(ValueMatcher.Matches(uriPattern, new Uri("http://httpbin.com:80"), defaultOptions));
        Assert.False(ValueMatcher.Matches(uriPattern, new Uri("http://httpbin.com:5000"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternPort_MatchesAnyUrlWithThatPort()
    {
        UriPattern uriPattern = new() { Port = new PatternValue("8*") };

        Assert.True(ValueMatcher.Matches(uriPattern, new Uri("http://httpbin.com"), defaultOptions));
        Assert.True(ValueMatcher.Matches(uriPattern, new Uri("http://httpbin.com:80"), defaultOptions));
        Assert.True(ValueMatcher.Matches(uriPattern, new Uri("http://httpbin.com:8443"), defaultOptions));
        Assert.False(ValueMatcher.Matches(uriPattern, new Uri("http://httpbin.com:5000"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern pattern = new() { Path = new ExactValue("/get") };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?query=test"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get/status"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/GET"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/post"), defaultOptions));

        Assert.False(ValueMatcher.Matches(new UriPattern { Path = new ExactValue("/test/../example") }, new Uri("http://httpbin.com/example"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternPath_MatchesAnyUrlWithThatExactPath()
    {
        UriPattern pattern = new() { Path = new PatternValue("/get*") };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?query=test"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get/status"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/GET"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/GET"), new() { PathCaseInsensitive = false }));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/post"), defaultOptions));
    }

    [Fact]
    public void Matches_ExactQuery_MatchesAnyUrlWithThatExactQuery()
    {
        UriPattern pattern = new() { Query = new ExactValue("key=value") };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?key=value"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get"), defaultOptions));
    }

    [Fact]
    public void Matches_PatternQuery_MatchesAnyUrlWithMatchinQuery()
    {
        UriPattern pattern = new() { Query = new PatternValue("*=value") };
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?key=value"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?key=test&query=value"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?KEY=test&QUERY=value"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?key=query"), defaultOptions));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?key=test&query=VALUE"), new() { QueryCaseInsensitive = false }));
    }

    [Fact]
    public void Matches_ExactQueryWithSpecialCharacters_MatchesAccordingToUriFormat()
    {
        UriPattern pattern = new() { Query = new ExactValue("email=test@example.com") };

        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?email=test@example.com"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?email=test%40example.com"), defaultOptions));
        Assert.True(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?email=test@example.com"), new() { DefaultQueryFormat = UriFormat.UriEscaped }));
        Assert.False(ValueMatcher.Matches(pattern, new Uri("https://httpbin.com/get?email=test%40example.com"), new() { DefaultQueryFormat = UriFormat.UriEscaped }));
    }

}
