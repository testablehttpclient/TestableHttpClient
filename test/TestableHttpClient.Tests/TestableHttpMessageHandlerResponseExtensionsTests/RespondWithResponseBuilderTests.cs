namespace TestableHttpClient.Tests.TestableHttpMessageHandlerResponseExtensionsTests;

public class RespondWithResponseBuilderTests
{
#nullable disable
    [Fact]
    public void RespondWith_NullHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(builder => builder.WithHttpStatusCode(HttpStatusCode.BadRequest)));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void RespondWith_NullBuilder_ThrowsArgumentNullException()
    {
        using var sut = new TestableHttpMessageHandler();
        Action<HttpResponseMessageBuilder> responseBuilder = null;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(responseBuilder));
        Assert.Equal("httpResponseMessageBuilderAction", exception.ParamName);
    }
#nullable restore

    [Fact]
    public async Task RespondWith_EmptyResponseBuilder_SetsDefaultResponse()
    {
        using var sut = new TestableHttpMessageHandler();
        sut.RespondWith(_ => { });

        using var client = new HttpClient(sut);

        var result = await client.GetAsync(new Uri("https://example.com/"));

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task RespondWith_UsingResponseBuilder_SetsModifiedResponse()
    {
        using var sut = new TestableHttpMessageHandler();
        sut.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.BadRequest));

        using var client = new HttpClient(sut);

        var result = await client.GetAsync(new Uri("https://example.com/"));

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal(string.Empty, await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task RespondWith_UsingResponseBuilder_EachRequestGetsANewResponse()
    {
        using var sut = new TestableHttpMessageHandler();
        sut.RespondWith(builder => builder.WithHttpStatusCode(HttpStatusCode.Unauthorized));

        using var client = new HttpClient(sut);

        var response1 = await client.GetAsync(new Uri("https://example.com/1"));
        var response2 = await client.GetAsync(new Uri("https://example.com/2"));

        Assert.NotSame(response1, response2);
    }

    [Fact]
    public async Task RespondWith_UsingResponseBuilder_RequestMessageIsSetForEachResponse()
    {
        using var sut = new TestableHttpMessageHandler();
        sut.RespondWith(builder => builder.WithHttpStatusCode(HttpStatusCode.Unauthorized));

        using var client = new HttpClient(sut);

        using var request1 = new HttpRequestMessage(HttpMethod.Get, new Uri("https://example.com/1"));
        using var request2 = new HttpRequestMessage(HttpMethod.Post, new Uri("https://example.com/2"));

        var response1 = await client.SendAsync(request1);
        var response2 = await client.SendAsync(request2);

        Assert.Same(request1, response1.RequestMessage);
        Assert.Same(request2, response2.RequestMessage);
        Assert.NotSame(response1.RequestMessage, response2.RequestMessage);
    }

    [Fact]
    public async Task RespondWith_UsingResponseBuilde_SetRequestMessageIsNotOverwritten()
    {
        using var sut = new TestableHttpMessageHandler();
        sut.RespondWith(builder => builder.WithRequestMessage(null));

        using var client = new HttpClient(sut);

        var response = await client.GetAsync(new Uri("https://example.com"));

        Assert.Null(response.RequestMessage);
    }
}
