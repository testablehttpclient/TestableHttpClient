namespace TestableHttpClient;

public sealed class TestableHttpMessageHandlerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions()
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [Obsolete("Renamed to UriPatternMatchingOptions")]
    public RoutingOptions RoutingOptions => UriPatternMatchingOptions;
    public UriPatternMatchingOptions UriPatternMatchingOptions { get; } = new UriPatternMatchingOptions();
}
