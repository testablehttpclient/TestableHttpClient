#if NET
using Microsoft.Extensions.Http;

using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.IntegrationTests;

public class TestingRetryMechanisms
{
    [Fact]
    public async Task TestingRetryPolicies()
    {
        // Create TestableHttpMessageHandler as usual.
        using TestableHttpMessageHandler testableHttpMessageHandler = new();
        testableHttpMessageHandler.RespondWith(
            Sequenced(
                StatusCode(HttpStatusCode.ServiceUnavailable),
                StatusCode(HttpStatusCode.ServiceUnavailable),
                StatusCode(HttpStatusCode.OK)
                ));

        // Configure the retry policy
        AsyncRetryPolicy<HttpResponseMessage> policy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(2);
        using PolicyHttpMessageHandler retryPolicyHandler = new(policy);

        using HttpClient client = testableHttpMessageHandler.CreateClient(retryPolicyHandler);

        // Make a request, which should pass
        HttpResponseMessage response = await client.GetAsync("https://httpbin.com/get", TestContext.Current.CancellationToken);

        // Now use the assertions to make sure the request was actually made multiple times.
        _ = testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get", 3);

        // Make sure the response is correct
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public void SimulateTimeoutDoesNotRetry()
    {
        // Create TestableHttpMessageHandler as usual.
        using TestableHttpMessageHandler testableHttpMessageHandler = new();
        testableHttpMessageHandler.RespondWith(Timeout());

        // Configure the retry policy
        AsyncRetryPolicy<HttpResponseMessage> policy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(2);
        using PolicyHttpMessageHandler retryPolicyHandler = new(policy);

        using HttpClient client = testableHttpMessageHandler.CreateClient(retryPolicyHandler);


        Task<HttpResponseMessage> task = client.GetAsync("https://httpbin.com/get", TestContext.Current.CancellationToken);
        Assert.True(task.IsCanceled);

        // Now use the assertions to make sure the request was actually made once, so polly didn't run.
        _ = testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get", 1);
    }
}
#endif
