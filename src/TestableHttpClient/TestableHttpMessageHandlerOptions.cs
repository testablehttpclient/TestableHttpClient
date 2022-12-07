using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient;

public sealed class TestableHttpMessageHandlerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new JsonSerializerOptions()
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [ExcludeFromCodeCoverage]
    [Obsolete("Renamed to UriPatternMatchingOptions", true)]
    public RoutingOptions RoutingOptions => new();
    public UriPatternMatchingOptions UriPatternMatchingOptions { get; } = new UriPatternMatchingOptions();
}
