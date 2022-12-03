using Moq;

namespace TestableHttpClient.Tests;

public partial class TestableHttpMessageHandlerExtensionsTests
{
    [Fact]
    public void CreateClientWithConfigurer_NullTestableHttpMessageHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        static void configureClient(HttpClient client) { }

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurer_NullConfigureAction_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();
        Action<HttpClient> configureClient = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient));
        Assert.Equal("configureClient", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurer_ReturnsHttpClientWithHandlerConfigured()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient client) { }

        using var client = sut.CreateClient(configureClient);

        var handler = GetPrivateHandler(client);

        Assert.Same(sut, handler);
    }

    [Fact]
    public void CreateClientWithConfigurer_CallsConfigureClientWithClientToReturn()
    {
        using TestableHttpMessageHandler sut = new();
        Mock<Action<HttpClient>> configureClient = new();

        using var client = sut.CreateClient(configureClient.Object);

        configureClient.Verify(x => x.Invoke(client), Times.Once);
    }
}
