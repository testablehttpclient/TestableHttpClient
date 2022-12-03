using Moq;

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
    public void CreateClientWithConfigurerAndHtppMessageHandlers_CallsConfigureClientWithClientToReturn()
    {
        using TestableHttpMessageHandler sut = new();
        var configureClient = Mock.Of<Action<HttpClient>>();
        var handlers = Enumerable.Empty<DelegatingHandler>();

        using var client = sut.CreateClient(configureClient, handlers);

        Mock.Get(configureClient).Verify(x => x.Invoke(client), Times.Once);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_WhenHttpMessageHandlersArePassed_HandlersAreCorrectlyLinked()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient _) { }
        IEnumerable<DelegatingHandler> handlers = new DelegatingHandler[]
        {
            Mock.Of<DelegatingHandler>(),
            Mock.Of<DelegatingHandler>()
        };

        using var client = sut.CreateClient(configureClient, handlers);

        var handler = GetPrivateHandler(client);
        var delegatingHandler1 = Assert.IsAssignableFrom<DelegatingHandler>(handler);
        Assert.Same(handlers.First(), delegatingHandler1);
        var delegatingHandler2 = Assert.IsAssignableFrom<DelegatingHandler>(delegatingHandler1.InnerHandler);
        Assert.Same(handlers.Last(), delegatingHandler2);
        Assert.Same(sut, delegatingHandler2.InnerHandler);
    }
}

