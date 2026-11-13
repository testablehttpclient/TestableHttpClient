using System.Text.Json;

using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests;

[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "https://github.com/dotnet/sdk/issues/57085")]
public sealed class RequestBuilderExtensionsTests
{
    private readonly RequestBuilder sut = new(new());

    [Fact]
    public void Get_WithNullBuilder_ShouldThrowArgumentNullException()
    {
        RequestBuilder sut = null!;
        Assert.Throws<ArgumentNullException>(() => sut.Get("https://localhost"));
    }

    [Fact]
    public void Get_WithNullRequestUri_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.Get(null!));
    }

    [Fact]
    public void Get_ShouldSetHttpMethodAndRequestUri()
    {
        Request request = sut.Get("https://localhost").Build();

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(UriPatternParser.Parse("https://localhost"), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equal(new Any(), request.Headers);
        Assert.Equal(new Any(), request.Content);
    }

    [Fact]
    public void Post_WithNullBuilder_ShouldThrowArgumentNullException()
    {
        RequestBuilder sut = null!;
        Assert.Throws<ArgumentNullException>(() => sut.Post("https://localhost", "{\"hello\":1}"));
    }

    [Fact]
    public void Post_WithNullRequestUri_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.Post(null!, "{\"hello\":1}"));
    }

    [Fact]
    public void Post_WithNullContent_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.Post("https://localhost", null!));
    }

    [Fact]
    public void Post_ShouldBuildCorrectRequest()
    {
        Request request = sut.Post("https://localhost", "{\"hello\":1}").Build();

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(UriPatternParser.Parse("https://localhost"), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equal(new Any(), request.Headers);
        Assert.Equal(new PatternValue("{\"hello\":1}"), request.Content);
    }

    [Fact]
    public void PostAsJson_WithNullBuilder_ShouldThrowArgumentNullException()
    {
        RequestBuilder sut = null!;

        Assert.Throws<ArgumentNullException>(() => sut.PostAsJson("https://localhost", new()));
    }

    [Fact]
    public void PostAsJson_WithNullRequestUri_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.PostAsJson(null!, new()));
    }

    [Fact]
    public void PostAsJson_WithNullContent_ShouldBuildCorrectRequest()
    {
        Request request = sut.PostAsJson("https://localhost", null).Build();

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(UriPatternParser.Parse("https://localhost"), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equivalent(new Dictionary<string, HeaderValue>() { ["Content-Type"] = new PatternValue("application/json*") }, request.Headers is HeaderList headerValues ? headerValues : null);
        Assert.Equal(new PatternValue("null"), request.Content);
    }

    [Fact]
    public void PostAsJson_ShouldBuildCorrectRequest()
    {
        Request request = sut.PostAsJson("https://localhost", new { hello = 1 }).Build();

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(UriPatternParser.Parse("https://localhost"), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equivalent(new Dictionary<string, HeaderValue>() { ["Content-Type"] = new PatternValue("application/json*") }, request.Headers is HeaderList headerValues ? headerValues : null);
        Assert.Equal(new PatternValue("{\"hello\":1}"), request.Content);
    }

    [Fact]
    public void WithJsonContent_WithNullBuilder_ShouldThrowArgumentNullException()
    {
        RequestBuilder sut = null!;

        Assert.Throws<ArgumentNullException>(() => sut.WithJsonContent(new()));
    }

    [Fact]
    public void WithJsonContent_WithNullContent_ShouldBuildCorrectRequest()
    {
        Request request = sut.WithJsonContent(null).Build();

        Assert.Equal(new Any(), request.Method);
        Assert.Equal(new UriPattern(), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equivalent(new Dictionary<string, HeaderValue>() { ["Content-Type"] = new PatternValue("application/json*") }, request.Headers is HeaderList headerValues ? headerValues : null);
        Assert.Equal(new PatternValue("null"), request.Content);
    }

    [Fact]
    public void WithJsonContent_ShouldBuildCorrectRequest()
    {
        Request request = sut.WithJsonContent(new { hello = 1 }).Build();

        Assert.Equal(new Any(), request.Method);
        Assert.Equal(new UriPattern(), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equivalent(new Dictionary<string, HeaderValue>() { ["Content-Type"] = new PatternValue("application/json*") }, request.Headers is HeaderList headerValues ? headerValues : null);
        Assert.Equal(new PatternValue("{\"hello\":1}"), request.Content);
    }

    [Fact]
    public void WithJsonContent_WithCustomOptions_ShouldBuildCorrectRequest()
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        Request request = sut.WithJsonContent(new { hello = 1 }, options).Build();

        Assert.Equal(new Any(), request.Method);
        Assert.Equal(new UriPattern(), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equivalent(new Dictionary<string, HeaderValue>() { ["Content-Type"] = new PatternValue("application/json*") }, request.Headers is HeaderList headerValues ? headerValues : null);
        Assert.Equal(new PatternValue("""
            {
              "hello": 1
            }
            """), request.Content);
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNullBuilder_ShouldThrowArgumentNullException()
    {
        RequestBuilder sut = null!;

        Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent([]));
    }

    [Fact]
    public void WithFormUrlEncodedContent_WithNullFormValues_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.WithFormUrlEncodedContent(null!));
    }

    [Fact]
    public void WithFormUrlEncodedContent_ShouldBuildCorrectRequest()
    {
        Request request = sut.WithFormUrlEncodedContent([new KeyValuePair<string?, string?>("username", "alice")]).Build();

        Assert.Equal(new Any(), request.Method);
        Assert.Equal(new UriPattern(), request.RequestUri);
        Assert.Equal(new Any(), request.Version);
        Assert.Equivalent(new Dictionary<string, HeaderValue>() { ["Content-Type"] = new PatternValue("application/x-www-form-urlencoded*") }, request.Headers is HeaderList headerValues ? headerValues : null);
        Assert.Equal(new PatternValue("username=alice"), request.Content);
    }
}
