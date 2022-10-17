using System.Text.Json;

namespace TestableHttpClient.Tests;

public class TestableHttpMessageHandlerOptionsTests
{
    [Fact]
    public void Constructor_ByDefault_SetsSaneDefaults()
    {
        TestableHttpMessageHandlerOptions options = new();

        Assert.NotNull(options.JsonSerializerOptions);
        Assert.Equal(JsonNamingPolicy.CamelCase, options.JsonSerializerOptions.DictionaryKeyPolicy);
        Assert.Equal(JsonNamingPolicy.CamelCase, options.JsonSerializerOptions.PropertyNamingPolicy);
    }
}
