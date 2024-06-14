using NSubstitute;

namespace TestableHttpClient.Tests;

public partial class TestableHttpMessageHandlerExtensionsTests
{
    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullTestableHttpMessageHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        static void configureClient(HttpClient _) { }
        var handlers = Enumerable.Empty<DelegatingHandler>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, handlers));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullConfigureAction_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();
        Action<HttpClient> configureClient = null!;
        var handlers = Enumerable.Empty<DelegatingHandler>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, handlers));
        Assert.Equal("configureClient", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullHttpMessageHandlers_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();

        static void configureClient(HttpClient _) { }
        IEnumerable<DelegatingHandler> handlers = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, handlers));
        Assert.Equal("httpMessageHandlers", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_CallsConfigureClientWithClientToReturn()
    {
        using TestableHttpMessageHandler sut = new();
        var configureClient = Substitute.For<Action<HttpClient>>();
        var handlers = Enumerable.Empty<DelegatingHandler>();

        using var client = sut.CreateClient(configureClient, handlers);

        configureClient.Received(1).Invoke(client);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_WhenHttpMessageHandlersArePassed_HandlersAreCorrectlyLinked()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient _) { }
        IEnumerable<DelegatingHandler> handlers =
        [
            Substitute.For<DelegatingHandler>(),
            Substitute.For<DelegatingHandler>()
        ];

        using var client = sut.CreateClient(configureClient, handlers);

        var handler = GetPrivateHandler(client);
        var delegatingHandler1 = Assert.IsAssignableFrom<DelegatingHandler>(handler);
        Assert.Same(handlers.First(), delegatingHandler1);
        var delegatingHandler2 = Assert.IsAssignableFrom<DelegatingHandler>(delegatingHandler1.InnerHandler);
        Assert.Same(handlers.Last(), delegatingHandler2);
        Assert.Same(sut, delegatingHandler2.InnerHandler);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_ByDefault_SetsDefaultBaseAddress()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient client) { }
        IEnumerable<DelegatingHandler> handlers =
        [
            Substitute.For<DelegatingHandler>(),
            Substitute.For<DelegatingHandler>()
        ];

        using var client = sut.CreateClient(configureClient, handlers);

        Assert.Equal(new Uri("https://localhost"), client.BaseAddress);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_WhenConfiguringBaseAddress_DoesNotOverrideWithDefault()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient client) { client.BaseAddress = new Uri("https://example"); }
        IEnumerable<DelegatingHandler> handlers =
        [
            Substitute.For<DelegatingHandler>(),
            Substitute.For<DelegatingHandler>()
        ];

        using var client = sut.CreateClient(configureClient, handlers);

        Assert.Equal(new Uri("https://example"), client.BaseAddress);
    }
}

