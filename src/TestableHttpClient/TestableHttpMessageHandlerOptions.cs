namespace TestableHttpClient;

public sealed class TestableHttpMessageHandlerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions()
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public UriPatternMatchingOptions UriPatternMatchingOptions { get; } = new UriPatternMatchingOptions();
}
