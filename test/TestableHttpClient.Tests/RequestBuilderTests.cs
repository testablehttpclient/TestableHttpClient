using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests;

public sealed class RequestBuilderTests
{
    private readonly RequestBuilder sut = new(new UriPatternMatchingOptions());

    [Fact]
    public void Build_ByDefault_CreatesEmptyRequest()
    {
        Request request = sut.Build();

        Assert.Null(request.Method);
        Assert.Null(request.RequestUri);
        Assert.Null(request.Version);
        Assert.Null(request.Headers);
        Assert.Null(request.Content);
    }

    [Fact]
    public void WithMethod_NullMethod_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.WithMethod(null!));
    }

    [Fact]
    public void WithMethod_CreatesRequestWithMethod()
    {
        Request request = sut.WithMethod(HttpMethod.Post).Build();

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Null(request.RequestUri);
        Assert.Null(request.Version);
        Assert.Null(request.Headers);
        Assert.Null(request.Content);
    }

    [Fact]
    public void WithRequestUri_NullUri_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.WithRequestUri(null!));
    }

    [Fact]
    public void WithRequestUri_EmptyUri_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => sut.WithRequestUri(string.Empty));
    }

    [Fact]
    public void WithRequestUri_UriPattern_ShouldSetRequestUri()
    {
        Request request = sut.WithRequestUri("http*//test.example").Build();

        Assert.Null(request.Method);
        Assert.Equal(UriPatternParser.Parse("http*//test.example"), request.RequestUri);
        Assert.Null(request.Version);
        Assert.Null(request.Headers);
        Assert.Null(request.Content);
    }

    [Fact]
    public void WithVersion_NullVersion_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.WithVersion(null!));
    }

    [Fact]
    public void WithVersion_CreatesRequestWithVersion()
    {
        Request request = sut.WithVersion(HttpVersion.Version11).Build();

        Assert.Null(request.Method);
        Assert.Null(request.RequestUri);
        Assert.Equal(HttpVersion.Version11, request.Version);
        Assert.Null(request.Headers);
        Assert.Null(request.Content);
    }

    [Fact]
    public void WithHeader_NullHeaderName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.WithHeader(null!));
        Assert.Throws<ArgumentNullException>(() => sut.WithHeader(null!, "test"));
    }

    [Fact]
    public void WithHeader_EmptyHeaderName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => sut.WithHeader(string.Empty));
        Assert.Throws<ArgumentException>(() => sut.WithHeader(string.Empty, "test"));
    }

    [Fact]
    public void WithHeader_ValidHeaderNameNoValue_CreatesRequestWithHeaderWithAnyValue()
    {
        Request request = sut.WithHeader("Content-Length").Build();

        Assert.Null(request.Method);
        Assert.Null(request.RequestUri);
        Assert.Null(request.Version);
        Assert.Equal(new Dictionary<string, Value>() { ["Content-Length"] = Value.Any() }, request.Headers);
        Assert.Null(request.Content);
    }

    [Fact]
    public void WithHeader_NullPattern_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.WithHeader("Content-Length", null!));
    }

    [Fact]
    public void WithHeader_EmptyPattern_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => sut.WithHeader("Content-Length", string.Empty));
    }

    [Fact]
    public void WithHeader_ValidHeaderNamePatternValue_CreatesRequestWithHeaderWithAnyValue()
    {
        Request request = sut.WithHeader("Content-Length", "*").Build();

        Assert.Null(request.Method);
        Assert.Null(request.RequestUri);
        Assert.Null(request.Version);
        Assert.Equal(new Dictionary<string, Value>() { ["Content-Length"] = Value.Pattern("*") }, request.Headers);
        Assert.Null(request.Content);
    }

    [Fact]
    public void WithContent_NullContent_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => sut.WithContent(null!));
    }

    [Fact]
    public void WithContent_CreatesRequestWithContent()
    {
        Request request = sut.WithContent("content").Build();

        Assert.Null(request.Method);
        Assert.Null(request.RequestUri);
        Assert.Null(request.Version);
        Assert.Null(request.Headers);
        Assert.Equal("content", request.Content);
    }
}
