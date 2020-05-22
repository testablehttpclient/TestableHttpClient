using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Xunit;

namespace TestableHttpClient.IntegrationTests
{
    public class HttpResponseMessageCustomChecks
    {
        [Fact]
        public void ExampleOfCustomAssertionsWithHttpResponseMessages()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Headers.Server.Add(new ProductInfoHeaderValue(product: new ProductHeaderValue("nginx")));
            httpResponseMessage.Content = new StringContent("{}", null, "application/json");

            Assert.True(httpResponseMessage.HasHttpStatusCode(HttpStatusCode.OK));
            Assert.True(httpResponseMessage.HasResponseHeader("Server"));
            Assert.True(httpResponseMessage.HasContent("{*}"));
            Assert.True(httpResponseMessage.HasContentHeader("Content-Type", "*/json*"));
        }
    }
}
