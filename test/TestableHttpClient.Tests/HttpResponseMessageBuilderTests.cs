namespace TestableHttpClient.Tests;

[Obsolete("Use ConfiguredResponse or a custom IResponse instead.")]
public class HttpResponseMessageBuilderTests
{
    [Fact]
    public void Build_ReturnsEmptyHttpResponseMessage()
    {
        HttpResponseMessageBuilder sut = new();

        var result = sut.Build();

        Assert.NotNull(result);
        Assert.Equal(HttpVersion.Version11, result.Version);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Empty(result.Headers);
        Assert.NotNull(result.Content);
        Assert.Null(result.RequestMessage);
    }

    [Fact]
    [Obsolete("Build is deprecated")]
    public void Build_ReturnsSameResponseMessage()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        var result = sut.Build();

        Assert.Same(responseMessage, result);
    }

    [Fact]
    public void Constructor_NullResponseMessage_ThrowsArgumentNullException()
    {
        HttpResponseMessage responseMessage = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => new HttpResponseMessageBuilder(responseMessage));
        Assert.Equal("httpResponseMessage", exception.ParamName);
    }

    [Fact]
    public void WithHttpVersion_CreatesHttpResponseMessageWithCorrectVersion()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithHttpVersion(HttpVersion.Version11);

        Assert.Equal(HttpVersion.Version11, responseMessage.Version);
    }

    [Fact]
    public void WithHttpStatusCode_CreatesHttpResponseMessageWithCorrectStatusCode()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithHttpStatusCode(HttpStatusCode.BadRequest);

        Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
    }

    [Fact]
    public void WithResponseHeaders_WhenPassingNull_ArgumentNullExceptionIsThrown()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithResponseHeaders(null!));
        Assert.Equal("responseHeaderBuilder", exception.ParamName);
    }

    [Fact]
    public void WithResponseHeaders_CreatesHttpResponseMessageWithCorrectHeaders()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithResponseHeaders(x => x.Location = new Uri("https://example.com/"));

        Assert.Equal("https://example.com/", responseMessage.Headers.Location?.AbsoluteUri);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void WithResponseHeader_NullOrEmptyName_ThrowsArgumentException(string headerName)
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithResponseHeader(headerName, "value"));
        Assert.Equal("header", exception.ParamName);
    }

    [Fact]
    public void WithResponseHeader_CreatesHttpResponseMessageWithHeaderAddedToTheList()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithResponseHeader("Location", "https://example.com/");

        Assert.Equal("https://example.com/", responseMessage.Headers.GetValues("Location").Single());
    }

    [Fact]
    public void WithContent_CreatesHttpResponseMessageWithContent()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);
        using var content = new StringContent(string.Empty);

        sut.WithContent(content);

        Assert.Same(content, responseMessage.Content);
    }

    [Fact]
    public void WithStringContent_NullContent_ThrowsArgumentNullException()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        var exception = Assert.Throws<ArgumentNullException>(() => sut.WithStringContent(null!));
        Assert.Equal("content", exception.ParamName);
    }

    [Fact]
    public async Task WithStringContent_CreatesHttpResponseMessageWithStringContent()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithStringContent("My content");

        Assert.Equal("My content", await responseMessage.Content.ReadAsStringAsync());
        Assert.Equal("text/plain", responseMessage.Content.Headers.ContentType?.MediaType);
        Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType?.CharSet);
    }

    [Fact]
    public void WithStringContent_WithNullEncoding_CreateHttpResponseMessageWithDefaultEncoding()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithStringContent("", null);

        Assert.Equal("text/plain", responseMessage.Content.Headers.ContentType?.MediaType);
        Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType?.CharSet);
    }

    [Fact]
    public void WithStringContent_WithEncoding_CreatesHttpResponseMessageWithContentTypeHeader()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithStringContent("", Encoding.ASCII);

        Assert.Equal("text/plain", responseMessage.Content.Headers.ContentType?.MediaType);
        Assert.Equal("us-ascii", responseMessage.Content.Headers.ContentType?.CharSet);
    }

    [Fact]
    public void WithStringContent_WithNullMediaType_CreateHttpResponseMessageWithDefaultMediaType()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithStringContent("", null, null!);

        Assert.Equal("text/plain", responseMessage.Content.Headers.ContentType?.MediaType);
        Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType?.CharSet);
    }

    [Fact]
    public void WithStringContent_WithMediaType_CreatesHttpResponseMessageWithContentTypeHeader()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithStringContent("", null, "application/json");

        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
        Assert.Equal("utf-8", responseMessage.Content.Headers.ContentType?.CharSet);
    }

    [Fact]
    public async Task WithJsonContent_Null_CreatesHttpResponseMessageWithNullJsonAndDefaultContentType()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithJsonContent(null);

        Assert.Equal("null", await responseMessage.Content.ReadAsStringAsync());
        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task WithJsonContent_ObjectAndNullMediaType_CreatesHttpResponseMessageWithJsonAndDefaultContentType()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithJsonContent(Array.Empty<object>(), null);

        Assert.Equal("[]", await responseMessage.Content.ReadAsStringAsync());
        Assert.Equal("application/json", responseMessage.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task WithJsonContent_ObjectAnCustomMediaType_CreatesHttpResponseMessageWithJsonAndContentType()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);

        sut.WithJsonContent(new { }, "text/json");

        Assert.Equal("{}", await responseMessage.Content.ReadAsStringAsync());
        Assert.Equal("text/json", responseMessage.Content.Headers.ContentType?.MediaType);
        Assert.Null(responseMessage.Content.Headers.ContentType?.CharSet);
    }

    [Fact]
    public void WithRequestMessage_CreatesHttpResponseMessagaeWithRequestMessage()
    {
        using HttpResponseMessage responseMessage = new();
        HttpResponseMessageBuilder sut = new(responseMessage);
        using var requestMessage = new HttpRequestMessage();

        sut.WithRequestMessage(requestMessage);

        Assert.Same(requestMessage, responseMessage.RequestMessage);
    }
}
