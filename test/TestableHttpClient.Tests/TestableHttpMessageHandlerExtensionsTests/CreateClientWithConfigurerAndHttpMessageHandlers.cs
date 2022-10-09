﻿using Moq;

namespace TestableHttpClient.Tests;

public partial class TestableHttpMessageHandlerExtensionsTests
{
    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullTestableHttpMessageHandler_ThrowsArgumentNullException()
    {
        TestableHttpMessageHandler sut = null!;
        Action<HttpClient> configureClient = _ => { };
        IEnumerable<DelegatingHandler> handlers = Enumerable.Empty<DelegatingHandler>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, handlers));
        Assert.Equal("handler", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullConfigureAction_ThrowsArgumentNullException()
    {
        using var sut = new TestableHttpMessageHandler();
        Action<HttpClient> configureClient = null!;
        IEnumerable<DelegatingHandler> handlers = Enumerable.Empty<DelegatingHandler>();

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, handlers));
        Assert.Equal("configureClient", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_NullHttpMessageHandlers_ThrowsArgumentNullException()
    {
        using var sut = new TestableHttpMessageHandler();
        Action<HttpClient> configureClient = _ => { };
        IEnumerable<DelegatingHandler> handlers = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient, handlers));
        Assert.Equal("httpMessageHandlers", exception.ParamName);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHtppMessageHandlers_CallsConfigureClientWithClientToReturn()
    {
        using var sut = new TestableHttpMessageHandler();
        Action<HttpClient> configureClient = Mock.Of<Action<HttpClient>>();
        IEnumerable<DelegatingHandler> handlers = Enumerable.Empty<DelegatingHandler>();

        using var client = sut.CreateClient(configureClient, handlers);

        Mock.Get(configureClient).Verify(x => x.Invoke(client), Times.Once);
    }

    [Fact]
    public void CreateClientWithConfigurerAndHttpMessageHandlers_WhenHttpMessageHandlersArePassed_HandlersAreCorrectlyLinked()
    {
        using var sut = new TestableHttpMessageHandler();
        Action<HttpClient> configureClient = _ => { };
        IEnumerable<DelegatingHandler> handlers = new DelegatingHandler[]
        {
            Mock.Of<DelegatingHandler>(),
            Mock.Of<DelegatingHandler>()
        };

        using var client = sut.CreateClient(_ => { }, handlers);

        var handler = GetPrivateHandler(client);
        var delegatingHandler1 = Assert.IsAssignableFrom<DelegatingHandler>(handler);
        Assert.Same(handlers.First(), delegatingHandler1);
        var delegatingHandler2 = Assert.IsAssignableFrom<DelegatingHandler>(delegatingHandler1.InnerHandler);
        Assert.Same(handlers.Last(), delegatingHandler2);
        Assert.Same(sut, delegatingHandler2.InnerHandler);
    }
}
