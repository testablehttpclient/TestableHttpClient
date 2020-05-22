using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using NFluent;

using TestableHttpClient.NFluent;

using Xunit;

namespace TestableHttpClient.IntegrationTests
{
    public class HttpResponseMessageNFluentChecks
    {
        [Fact]
        public void ExampleOfNFluentChecksForHttpResponseMessages()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Headers.Server.Add(new ProductInfoHeaderValue(new ProductHeaderValue("nginx")));
            httpResponseMessage.Content = new StringContent("{}", null, "application/json");

            Check.That(httpResponseMessage).HasHttpStatusCode(HttpStatusCode.OK)
                .And.HasResponseHeader("Server")
                .And.HasContent()
                .And.HasContentHeader("Content-Type", "*/json*")
                .And.HasContent("{*}");
        }
    }
}
