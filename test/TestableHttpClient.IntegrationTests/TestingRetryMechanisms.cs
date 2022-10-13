#if NETCOREAPP || NET
using Microsoft.Extensions.Http;

using Polly;
using Polly.Extensions.Http;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.IntegrationTests;

public class TestingRetryMechanisms
{
    [Fact]
    public async Task TestingRetryPolicies()
    {
        // Create TestableHttpMessageHandler as usual.
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        testableHttpMessageHandler.RespondWith(StatusCode(HttpStatusCode.ServiceUnavailable));

        // Configure the retry policy
        var policy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(2);
        using PolicyHttpMessageHandler retryPolicyHandler = new(policy);

        using var client = testableHttpMessageHandler.CreateClient(retryPolicyHandler);

        // Make a request, which should fail
        _ = await client.GetAsync("https://httpbin.com/get");

        // Now use the assertions to make sure the request was actually made multiple times.
        _ = testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get", 3);
    }

    [Fact]
    public void SimulateTimeoutDoesNotRetry()
    {
        // Create TestableHttpMessageHandler as usual.
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        testableHttpMessageHandler.RespondWith(Timeout());

        // Configure the retry policy
        var policy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(2);
        using PolicyHttpMessageHandler retryPolicyHandler = new(policy);

        using var client = testableHttpMessageHandler.CreateClient(retryPolicyHandler);

        
        var task  = client.GetAsync("https://httpbin.com/get");
        Assert.True(task.IsCanceled);

        // Now use the assertions to make sure the request was actually made once, so polly didn't run.
        _ = testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get", 1);
    }
}
#endif
