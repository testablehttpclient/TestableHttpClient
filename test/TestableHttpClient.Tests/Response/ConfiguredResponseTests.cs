using System.Threading;

using TestableHttpClient.Response;

namespace TestableHttpClient.Tests.Response;

public class ConfiguredResponseTests
{
    [Fact]
    public void Constructor_NullResponse_ThrowsArgumentNullException()
    {
        IResponse response = null!;
        Action<HttpResponseMessage> configureResponse = _ => { };
        var exception = Assert.Throws<ArgumentNullException>(() => new ConfiguredResponse(response, configureResponse));
        Assert.Equal("response", exception.ParamName);
    }

    [Fact]
    public void Constructor_NullConfigureResponse_ThrowsArgumentNullException()
    {
        HttpResponse response = new(HttpStatusCode.OK);
        Action<HttpResponseMessage> configureResponse = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => new ConfiguredResponse(response, configureResponse));
        Assert.Equal("configureResponse", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_ByDefault_CallsConfigureResponse()
    {
        bool wasCalled = false;
        HttpResponse response = new(HttpStatusCode.OK);
        Action<HttpResponseMessage> configureResponse = _ => wasCalled = true;
        using HttpRequestMessage requestMessage = new();
        using HttpResponseMessage responseMessage = new();
        ConfiguredResponse sut = new(response, configureResponse);

        await sut.ExecuteAsync(new HttpResponseContext(requestMessage, responseMessage), CancellationToken.None);

        Assert.True(wasCalled, "configureResponse action was not called.");
    }
}
