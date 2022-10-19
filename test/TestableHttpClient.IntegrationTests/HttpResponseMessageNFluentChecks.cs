namespace TestableHttpClient.IntegrationTests;

[Obsolete("Testing deptrecated functionality")]
public class HttpResponseMessageNFluentChecks
{
    [Fact]
    public void ExampleOfNFluentChecksForHttpResponseMessages()
    {
        using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        httpResponseMessage.Headers.Server.Add(new ProductInfoHeaderValue(new ProductHeaderValue("nginx")));
        httpResponseMessage.Content = new StringContent("{}", null, "application/json");

        Check.That(httpResponseMessage).HasHttpStatusCode(HttpStatusCode.OK)
            .And.HasResponseHeader("Server")
            .And.HasContent()
            .And.HasContentHeader("Content-Type", "*/json*")
            .And.HasContent("{*}");
    }
}
