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

    public static readonly TheoryData<string, string> CaseSensitivePatternValueHappyCases = new()
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

    public static readonly TheoryData<string, string> CaseInsensitivePatternValueHappyCases = new()
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

    public static readonly TheoryData<string, string> CaseSensitivePatternValueUnhappyCases = new()
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

    public static readonly TheoryData<string, string> CaseInsensitivePatternValueUnhappyCases = new()
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

    [Fact]
    public void Matches_AnyUriPattern_MatchesNullUri()
    {
        UriPattern pattern = new();
        Uri? uri = null;

        Assert.True(ValueMatcher.Matches(pattern, uri, defaultOptions));
    }

    [Fact]
    public void Matches_NotAnyPattern_DoesNotMatchNullUri()
    {
        UriPattern pattern = new() { Host = new ExactValue("example.com") };
        Uri? uri = null;

        Assert.False(ValueMatcher.Matches(pattern, uri, defaultOptions));
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

    [Fact]
    public void Matches_AnyRequest_MatchesAnyRequest()
    {
        Request expectedRequest = new();
        using HttpRequestMessage actualRequest = new(HttpMethod.Options, "https://example.com:444/test?value=test");
        actualRequest.Headers.Authorization = new("bearer", "blabla");
        actualRequest.Content = new StringContent("test", Encoding.UTF8);

        Assert.True(ValueMatcher.Matches(expectedRequest, actualRequest, defaultOptions));
    }

    [Fact]
    public void Matches_HttpMethod_MatchesExactHttpMethod()
    {
        Request expectedRequest = new() { Method = HttpMethod.Get };
        using HttpRequestMessage matchingRequest = new(HttpMethod.Get, "https://example.com");
        using HttpRequestMessage notMatchingRequest = new(HttpMethod.Post, "https://example.com");

        Assert.True(ValueMatcher.Matches(expectedRequest, matchingRequest, defaultOptions));
        Assert.False(ValueMatcher.Matches(expectedRequest, notMatchingRequest, defaultOptions));
    }

    [Fact]
    public void Matches_NullRequestUri_MatchesAny()
    {
        Request anyRequest = new();
        using HttpRequestMessage requestMessage = new();

        Assert.True(ValueMatcher.Matches(anyRequest, requestMessage, defaultOptions));
    }

    [Fact]
    public void Matches_NullRequestUri_DoesNotMatchNotExactValue()
    {
        Request anyRequest = new() { RequestUri = new() { Host = new ExactValue("example.com") } };
        using HttpRequestMessage requestMessage = new();

        Assert.False(ValueMatcher.Matches(anyRequest, requestMessage, defaultOptions));
    }

    [Fact]
    public void Matches_RequestUri_MatchesBasedOnOptions()
    {
        Request expectedRequest = new() { RequestUri = new() { Query = new ExactValue("email=test@example.com") } };
        using HttpRequestMessage requestWithNormalUri = new(HttpMethod.Get, "https://httpbin.com/get?email=test@example.com");
        using HttpRequestMessage requestWithEscapedUri = new(HttpMethod.Get, "https://httpbin.com/get?email=test%40example.com");

        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithNormalUri, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithEscapedUri, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithNormalUri, new() { DefaultQueryFormat = UriFormat.UriEscaped }));
        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithEscapedUri, new() { DefaultQueryFormat = UriFormat.UriEscaped }));
    }

    [Fact]
    public void Matches_Version_MatchesAnyVersion()
    {
        Request expectedRequest = new();
        using HttpRequestMessage requestWithVersion10 = new() { Version = HttpVersion.Version10 };
        using HttpRequestMessage requestWithVersion11 = new() { Version = HttpVersion.Version11 };

        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithVersion10, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithVersion11, defaultOptions));
    }

    [Fact]
    public void Matches_Version_MatchesExactVersion()
    {
        Request expectedRequest = new() { Version = HttpVersion.Version10 };
        using HttpRequestMessage requestWithVersion10 = new() { Version = HttpVersion.Version10 };
        using HttpRequestMessage requestWithVersion11 = new() { Version = HttpVersion.Version11 };

        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithVersion10, defaultOptions));
        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithVersion11, defaultOptions));
    }

    [Fact]
    public void Matches_Headers_NoneMatchesEmptyHeaders()
    {
        Request expectedRequest = new() { Headers = new None() };

        using HttpRequestMessage requestWithoutHeaders = new();
        using HttpRequestMessage requestWithHeaders = new();
        requestWithHeaders.Headers.Host = "localhost";

        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithoutHeaders, defaultOptions));
        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithHeaders, defaultOptions));
    }

    [Fact]
    public void Matches_Headers_MatchesAnyHeader()
    {
        Request expectedRequest = new() { Headers = new Any() };

        using HttpRequestMessage requestWithoutHeaders = new();
        using HttpRequestMessage requestWithHeaders = new();
        requestWithHeaders.Headers.AcceptLanguage.Add(new("nl"));
        requestWithHeaders.Headers.Authorization = new("Test", "blaat");
        requestWithHeaders.Headers.Host = "localhost";

        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithoutHeaders, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithHeaders, defaultOptions));
    }

    [Theory]
    [InlineData("host")]
    [InlineData("Host")]
    [InlineData("HOST")]
    public void Matches_Headers_MatchesHeaderNameAndAnyValue(string headerName)
    {
        Request expectedRequest = new();
        expectedRequest.AddHeader(headerName);

        using HttpRequestMessage requestWithoutHostHeader = new();
        using HttpRequestMessage requestWithHostHeader = new();
        requestWithHostHeader.Headers.Host = "Test";
        using HttpRequestMessage requestWithOtherHostHeader = new();
        requestWithOtherHostHeader.Headers.Host = "example.com";

        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithoutHostHeader, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithHostHeader, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithOtherHostHeader, defaultOptions));
    }

    [Theory]
    [InlineData("content-type")]
    [InlineData("Content-Type")]
    [InlineData("CONTENT-TYPE")]
    public void Matches_Headers_MatchesContentHeaderNameAndAnyValue(string headerName)
    {
        Request expectedRequest = new();
        expectedRequest.AddHeader(headerName);

        using HttpRequestMessage requestWithoutContentTypeHeader = new();
        using HttpRequestMessage requestWithContentTypeHeader = new()
        {
            Content = new StringContent("")
        };
        requestWithContentTypeHeader.Content.Headers.ContentType = new("application/json");

        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithoutContentTypeHeader, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithContentTypeHeader, defaultOptions));
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("*.com")]
    [InlineData("example.*")]
    public void Matches_Headers_MatchesValueWithExactAndPatternValues(string expectedValue)
    {
        Request expectedRequest = new();
        expectedRequest.AddHeader("host", expectedValue);

        using HttpRequestMessage requestWithNotMatchingValue = new();
        requestWithNotMatchingValue.Headers.Host = "Test";
        using HttpRequestMessage requestWithMatchingValue = new();
        requestWithMatchingValue.Headers.Host = "example.com";

        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithNotMatchingValue, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithMatchingValue, defaultOptions));
    }

    [Theory]
    [InlineData("example.com")]
    [InlineData("*.com")]
    [InlineData("example.*")]
    public void Matches_Headers_MatchesValueCaseSensitive(string expectedValue)
    {
        Request expectedRequest = new();
        expectedRequest.AddHeader("host", expectedValue);

        using HttpRequestMessage requestWithNotMatchingValue = new();
        requestWithNotMatchingValue.Headers.Host = "EXAMPLE.COM";

        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithNotMatchingValue, defaultOptions));
    }

    [Fact]
    public void Matches_Content_NoneMatchesNullAndEmptyContent()
    {
        Request expectedRequest = new() { Content = new None() };

        using HttpRequestMessage requestWithoutContent = new() { Content = null };
        using HttpRequestMessage requestWithEmptyContent = new() { Content = new StringContent("") };
        using HttpRequestMessage requestWithWhitspaceContent = new() { Content = new StringContent("  \r\n") };
        using HttpRequestMessage requestWithContent = new() { Content = new StringContent("test") };

        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithoutContent, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithEmptyContent, defaultOptions));
        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithWhitspaceContent, defaultOptions));
        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithContent, defaultOptions));
    }

    [Fact]
    public void Matches_Content_AnyMatchesAnyContent()
    {
        Request expectedRequest = new() { Content = new Any() };

        using HttpRequestMessage requestWithoutContent = new() { Content = null };
        using HttpRequestMessage requestWithContent = new() { Content = new StringContent("test") };

        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithoutContent, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithContent, defaultOptions));
    }

    [Theory]
    [InlineData("test")]
    [InlineData("*st")]
    [InlineData("te*")]
    [InlineData("t*t")]
    public void Matches_Content_PatternMatchesContentButNotNull(string pattern)
    {
        Request expectedRequest = new() { Content = new PatternValue(pattern) };

        using HttpRequestMessage requestWithoutContent = new() { Content = null };
        using HttpRequestMessage requestWithContent = new() { Content = new StringContent("test") };

        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithoutContent, defaultOptions));
        Assert.True(ValueMatcher.Matches(expectedRequest, requestWithContent, defaultOptions));
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("t.*t")]
    [InlineData("T*ST")]
    public void Matches_Content_PatternDoesNotMatchAlways(string pattern)
    {

        Request expectedRequest = new() { Content = new PatternValue(pattern) };

        using HttpRequestMessage requestWithContent = new() { Content = new StringContent("test") };

        Assert.False(ValueMatcher.Matches(expectedRequest, requestWithContent, defaultOptions));
    }
}
