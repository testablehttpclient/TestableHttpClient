#if NET
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static TestableHttpClient.Responses;

namespace TestableHttpClient.IntegrationTests;

public class UsingIHttpClientFactory
{
    [Fact]
    public async Task ConfigureIHttpClientFactoryToUseTestableHttpClient()
    {
        // Create TestableHttpMessageHandler as usual.
        using TestableHttpMessageHandler testableHttpMessageHandler = new();
        testableHttpMessageHandler.RespondWith(StatusCode(HttpStatusCode.NoContent));

        ServiceCollection services = new();
        // Register an HttpClient and configure the TestableHttpMessageHandler as the PrimaryHttpMessageHandler
        services.AddHttpClient(string.Empty).ConfigurePrimaryHttpMessageHandler(() => testableHttpMessageHandler);

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        // Request the IHttpClientFactory
        IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        // Create the HttpClient
        using HttpClient client = httpClientFactory.CreateClient();
        // And use it...
        _ = await client.GetAsync("https://httpbin.com/get", TestContext.Current.CancellationToken);

        // Now use the assertions to make sure the request was actually made.
        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get");

        // Since we already have created the serviceProvider, we can't (easily) replace the TestableHttpMessageHandler.
        // So in case you need to start fresh, i.e. when the serviceProvider is part of shared context, you can clear it.
        testableHttpMessageHandler.ClearRequests();

        testableHttpMessageHandler.ShouldHaveMadeRequests(0);
    }

    [Fact]
    public async Task ConfigureMultipleHttpMessageHandlers()
    {
        // Create multiple TestableHttpMessageHandlers as usual, if the response is not important, a single TestableHttpMessageHandler can be used.
        using TestableHttpMessageHandler testableGithubHandler = new();
        testableGithubHandler.RespondWith(Configured(Text(string.Empty), x => x.Headers.Add("Server", "github")));

        using TestableHttpMessageHandler testableHttpBinHandler = new();
        testableHttpBinHandler.RespondWith(Configured(StatusCode(HttpStatusCode.NotFound), x => x.Headers.Add("Server", "httpbin")));

        ServiceCollection services = new();
        // Register named HttpClients and configure the correct TestableHttpMessageHandler as the PrimaryHttpMessageHandler
        services.AddHttpClient("github").ConfigurePrimaryHttpMessageHandler(() => testableGithubHandler);
        services.AddHttpClient("httpbin").ConfigurePrimaryHttpMessageHandler(() => testableHttpBinHandler);
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        // Request the IHttpClientFactory
        IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Create the named HttpClient
        using HttpClient githubClient = httpClientFactory.CreateClient("github");
        // And use it.
        HttpResponseMessage result = await githubClient.GetAsync("https://github.com/api/users", TestContext.Current.CancellationToken);
        Assert.Equal("github", result.Headers.Server.ToString());
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        // Create another named HttpClient
        using HttpClient httpbinClient = httpClientFactory.CreateClient("httpbin");
        // And use it...
        result = await httpbinClient.GetAsync("https://httpbin.com/get", TestContext.Current.CancellationToken);
        Assert.Equal("httpbin", result.Headers.Server.ToString());
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

        // Now assert every TestableHttpMessageHandlers to make sure we made the requests.
        testableGithubHandler.ShouldHaveMadeRequestsTo("https://github.com/*");
        testableHttpBinHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/*");
    }

    [Fact]
    public async Task ConfigureViaConfigureTestServicesOnHostBuilder()
    {
        // Create TestableHttpMessageHandler as usual.
        using TestableHttpMessageHandler testableHttpMessageHandler = new();
        testableHttpMessageHandler.RespondWith(StatusCode(HttpStatusCode.NoContent));

        // Setup a TestServer
        using IHost host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer()
                    // Configure the startup class, in this case it already configures a default HttpClient
                    .UseStartup<StartUpWithDefaultHttpHandler>()
                    // Reconfigure the default HttpClient and set the TestableHttpMessageHandler as the primary HttpMessageHandler
                    .ConfigureTestServices(services => services.AddHttpClient(string.Empty).ConfigurePrimaryHttpMessageHandler(() => testableHttpMessageHandler));
            })
            .StartAsync(cancellationToken: TestContext.Current.CancellationToken);

        using HttpClient client = host.GetTestClient();
        // Make a request to the testserver
        _ = await client.GetAsync("/", TestContext.Current.CancellationToken);

        // Assert that the code in the test server made the expected request.
        testableHttpMessageHandler.ShouldHaveMadeRequestsTo("https://httpbin.com/get");
    }

    private sealed class StartUpWithDefaultHttpHandler
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                IHttpClientFactory httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                HttpClient httpClient = httpClientFactory.CreateClient();
                await httpClient.GetAsync("https://httpbin.com/get");
                await context.Response.WriteAsync("Hello");
            });
        }
    }
}
#endif
