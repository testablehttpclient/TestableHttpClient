using NSubstitute;

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
    public void CreateClientWithConfigurer_ByDefault_SetsDefaultBaseAddress()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient client) { }

        using var client = sut.CreateClient(configureClient);

        Assert.Equal(new Uri("https://localhost"), client.BaseAddress);
    }

    [Fact]
    public void CreateClientWithConfigurer_WhenConfiguringBaseAddress_DoesNotOverrideWithDefault()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient client) { client.BaseAddress = new Uri("https://example"); }

        using var client = sut.CreateClient(configureClient);

        Assert.Equal(new Uri("https://example"), client.BaseAddress);
    }

    [Fact]
    public void CreateClientWithConfigurer_CallsConfigureClientWithClientToReturn()
    {
        using TestableHttpMessageHandler sut = new();
        Action<HttpClient> configureClient = Substitute.For<Action<HttpClient>>();

        using var client = sut.CreateClient(configureClient);

        configureClient.Received(1).Invoke(client);
    }
}
