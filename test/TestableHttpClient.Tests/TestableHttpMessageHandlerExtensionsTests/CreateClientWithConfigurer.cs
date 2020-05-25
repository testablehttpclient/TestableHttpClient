using System;
using System.Net.Http;

using Moq;

using Xunit;

namespace TestableHttpClient.Tests
{
    public partial class TestableHttpMessageHandlerExtensionsTests
    {
#nullable disable
        [Fact]
        public void CreateClientWithConfigurer_NullTestableHttpMessageHandler_ThrowsArgumentNullException()
        {
            TestableHttpMessageHandler sut = null;
            Action<HttpClient> configureClient = client => { };

            var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient));
            Assert.Equal("handler", exception.ParamName);
        }

        [Fact]
        public void CreateClientWithConfigurer_NullConfigureAction_ThrowsArgumentNullException()
        {
            using var sut = new TestableHttpMessageHandler();
            Action<HttpClient> configureClient = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient(configureClient));
            Assert.Equal("configureClient", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void CreateClientWithConfigurer_ReturnsHttpClientWithHandlerConfigured()
        {
            using var sut = new TestableHttpMessageHandler();
            Action<HttpClient> configureClient = client => { };

            using var client = sut.CreateClient(configureClient);

            var handler = GetPrivateHandler(client);

            Assert.Same(sut, handler);
        }

        [Fact]
        public void CreateClientWithConfigurer_CallsConfigureClientWithClientToReturn()
        {
            using var sut = new TestableHttpMessageHandler();
            Mock<Action<HttpClient>> configureClient = new Mock<Action<HttpClient>>();

            using var client = sut.CreateClient(configureClient.Object);

            configureClient.Verify(x => x.Invoke(client), Times.Once);
        }
    }
}
