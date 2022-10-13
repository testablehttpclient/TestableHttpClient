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
        IResponse response = new StatusCodeResponse(HttpStatusCode.OK);
        Action<HttpResponseMessage> configureResponse = null!;
        var exception = Assert.Throws<ArgumentNullException>(() => new ConfiguredResponse(response, configureResponse));
        Assert.Equal("configureResponse", exception.ParamName);
    }
}
