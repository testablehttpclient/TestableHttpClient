using Microsoft.Extensions.Http;

using Polly;
using Polly.Extensions.Http;

namespace TestableHttpClient.IntegrationTests;

public class TestingRetryMechanisms
{
    [Fact]
    public async Task TestingRetryPolicies()
    {
        // Create TestableHttpMessageHandler as usual.
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        testableHttpMessageHandler.RespondWith(response => response.WithHttpStatusCode(HttpStatusCode.ServiceUnavailable));

        // Configure the retry policy
        var policy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(2);
        using PolicyHttpMessageHandler retryPolicyHandler = new(policy);

        using HttpClient client = testableHttpMessageHandler.CreateClient(retryPolicyHandler);

        // Make a request, which should fail
        _ = await client.GetAsync("https://httpbin.com/get");

        // Now use the assertions to make sure the request was actually made multiple times.
        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get", 3);
    }

    [Fact]
    public async Task SimulateTimeoutDoesNotRetry()
    {
        // Create TestableHttpMessageHandler as usual.
        using var testableHttpMessageHandler = new TestableHttpMessageHandler();
        testableHttpMessageHandler.SimulateTimeout();

        // Configure the retry policy
        var policy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(2);
        using PolicyHttpMessageHandler retryPolicyHandler = new(policy);

        using HttpClient client = testableHttpMessageHandler.CreateClient(retryPolicyHandler);

        try
        {
            _ = await client.GetAsync("https://httpbin.com/get");
            Assert.Fail("This should never be reached, since a timeout should throw an exception.");
        }
        catch (TaskCanceledException)
        {
            // Catch the TaskCanceledException, since we know we that is thrown when a timeout occurs
        }

        // Now use the assertions to make sure the request was actually made once, so polly didn't run.
        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get", 1);
    }
}
