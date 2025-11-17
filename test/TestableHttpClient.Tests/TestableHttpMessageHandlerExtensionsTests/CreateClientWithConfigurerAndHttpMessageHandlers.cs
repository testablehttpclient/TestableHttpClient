namespace TestableHttpClient.Tests;

public partial class TestableHttpMessageHandlerExtensionsTests
{
    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullTestableHttpMessageHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;

        static void configureClient(HttpClient _) { }

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, []));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullConfigureAction_ThrowsArgumentNullException()
    {
        using TestableHttpMessageHandler sut = new();
        Action<HttpClient> configureClient = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, []));
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
        HttpClient? capturedClient = null;
        using TestableHttpMessageHandler sut = new();
        void configureClient(HttpClient client) => capturedClient = client;

        using var client = sut.CreateClient(configureClient, []);

        Assert.Same(client, capturedClient);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_WhenHttpMessageHandlersArePassed_HandlersAreCorrectlyLinked()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient _) { }
        using TestDelegateHandler delegate1 = new();
        using TestDelegateHandler delegate2 = new();

        using var client = sut.CreateClient(configureClient, [delegate1, delegate2]);

        var handler = GetPrivateHandler(client);
        var delegatingHandler1 = Assert.IsAssignableFrom<DelegatingHandler>(handler);
        Assert.Same(delegate1, delegatingHandler1);
        var delegatingHandler2 = Assert.IsAssignableFrom<DelegatingHandler>(delegatingHandler1.InnerHandler);
        Assert.Same(delegate2, delegatingHandler2);
        Assert.Same(sut, delegatingHandler2.InnerHandler);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_ByDefault_SetsDefaultBaseAddress()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient client) { }
        using TestDelegateHandler delegate1 = new();
        using TestDelegateHandler delegate2 = new();

        using var client = sut.CreateClient(configureClient, [delegate1, delegate2]);

        Assert.Equal(new Uri("https://localhost"), client.BaseAddress);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_WhenConfiguringBaseAddress_DoesNotOverrideWithDefault()
    {
        using TestableHttpMessageHandler sut = new();
        static void configureClient(HttpClient client) { client.BaseAddress = new Uri("https://example"); }
        using TestDelegateHandler delegate1 = new();
        using TestDelegateHandler delegate2 = new();

        using var client = sut.CreateClient(configureClient, [delegate1, delegate2]);

        Assert.Equal(new Uri("https://example"), client.BaseAddress);
    }

    private sealed class TestDelegateHandler : DelegatingHandler { }
}

