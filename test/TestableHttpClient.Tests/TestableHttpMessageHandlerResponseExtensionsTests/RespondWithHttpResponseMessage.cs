using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace TestableHttpClient.Tests.TestableHttpMessageHandlerResponseExtensionsTests
{
    public class RespondWithHttpResponseMessage
    {
#nullable disable
        [Fact]
        public void RespondWith_NullHandler_ThrowsArgumentNullException()
        {
            TestableHttpMessageHandler sut = null;
            using var response = new HttpResponseMessage();

            var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(response));
            Assert.Equal("handler", exception.ParamName);
        }

        [Fact]
        public void RespondWith_NullHttpResponseMessage_ThrowsArgumentNullException()
        {
            using var sut = new TestableHttpMessageHandler();
            HttpResponseMessage response = null;

            var exception = Assert.Throws<ArgumentNullException>(() => sut.RespondWith(response));
            Assert.Equal("httpResponseMessage", exception.ParamName);
        }
#nullable restore

        [Fact]
        public async Task RespondWith_ByDefault_ReturnsSameResponseUnModified()
        {
            using var sut = new TestableHttpMessageHandler();
            using var response = new HttpResponseMessage();
            sut.RespondWith(response);
            using var client = new HttpClient(sut);

            var result = await client.GetAsync("https://example.com");

            Assert.Same(response, result);
            Assert.Null(result.RequestMessage);
        }
    }
}
