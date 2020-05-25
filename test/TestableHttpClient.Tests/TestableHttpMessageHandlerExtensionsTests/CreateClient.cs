using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

using Xunit;

namespace TestableHttpClient.Tests
{
    public partial class TestableHttpMessageHandlerExtensionsTests
    {
#nullable disable
        [Fact]
        public void CreateClient_NullTestableHttpMessageHandler_ThrowsArgumentNullException()
        {
            TestableHttpMessageHandler sut = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.CreateClient());
            Assert.Equal("handler", exception.ParamName);
        }
#nullable restore

        [Fact]
        public void CreateClient_CorrectTestableHttpMessageHandler_AddsHandlerToHttpClient()
        {
            using var sut = new TestableHttpMessageHandler();

            using var client = sut.CreateClient();

            var handler = GetPrivateHandler(client);

            Assert.Same(sut, handler);
        }

        private static object? GetPrivateHandler(HttpClient client)
        {
            var handlerField = client.GetType().BaseType?.GetField("_handler", BindingFlags.Instance | BindingFlags.NonPublic);
            if(handlerField == null)
            {
                Assert.True(false, "Can't find the private _handler field on HttpClient.");
                return null;
            }
            return handlerField.GetValue(client);
        }
    }
}
