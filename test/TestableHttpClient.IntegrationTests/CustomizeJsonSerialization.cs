using System.Net.Http.Json;
using System.Text.Json;

using static TestableHttpClient.Responses;

namespace TestableHttpClient.IntegrationTests;

public class CustomizeJsonSerialization
{
    [Fact]
    public async Task ByDefault_CamelCasing_is_used_for_json_serialization()
    {
        using TestableHttpMessageHandler sut = new();
        sut.RespondWith(Json(new { Name = "Charlie" }));
        using HttpClient client = sut.CreateClient();

        string json = await client.GetStringAsync("http://localhost/myjson");

        Assert.Equal("{\"name\":\"Charlie\"}", json);
    }

    [Fact]
    public async Task But_this_can_be_changed()
    {
        using TestableHttpMessageHandler sut = new();
        sut.Options.JsonSerializerOptions.PropertyNamingPolicy = null;
        sut.RespondWith(Json(new { Name = "Charlie" }));
        using HttpClient client = sut.CreateClient();

        string json = await client.GetStringAsync("http://localhost/myjson");

        Assert.Equal("{\"Name\":\"Charlie\"}", json);
    }

    [Fact]
    public async Task But_Also_directly_on_the_response()
    {
        using TestableHttpMessageHandler sut = new();
        sut.RespondWith(Json(new { Name = "Charlie" }, jsonSerializerOptions: new JsonSerializerOptions()));
        using HttpClient client = sut.CreateClient();

        string json = await client.GetStringAsync("http://localhost/myjson");

        Assert.Equal("{\"Name\":\"Charlie\"}", json);
    }

    [Fact]
    public async Task Asserting_also_works_this_way()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = sut.CreateClient();
        await client.PostAsJsonAsync("http://localhost", new { Name = "Charlie" }, cancellationToken: TestContext.Current.CancellationToken);

#if NETFRAMEWORK
        // Well this doesn't really work on .NET Framework.
        sut.ShouldHaveMadeRequests();
#else
        sut.ShouldHaveMadeRequests().WithJsonContent(new { Name = "Charlie" });
#endif
    }

    [Fact]
    public async Task And_we_can_go_crazy_with_it()
    {
        using TestableHttpMessageHandler sut = new();
        using HttpClient client = sut.CreateClient();
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        await client.PostAsJsonAsync("http://localhost", new { Name = "Charlie" }, options, cancellationToken: TestContext.Current.CancellationToken);

#if NETFRAMEWORK
        // Well this doesn't really work on .NET Framework.
        sut.ShouldHaveMadeRequests();
#else
        sut.ShouldHaveMadeRequests().WithJsonContent(new { Name = "Charlie" }, options);
#endif
    }
}
