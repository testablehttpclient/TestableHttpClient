namespace TestableHttpClient;

/// <summary>
/// Options specific for URI pattern matching.
/// </summary>
#pragma warning disable CS0618 // Type or member is obsolete, needed for backwards compatibility
public class UriPatternMatchingOptions : RoutingOptions
#pragma warning restore CS0618 // Type or member is obsolete
{
    /// <summary>
    /// Indicates whether or not the query parameters of an URI should be treated as case insensitive. Default: true
    /// </summary>
    public bool QueryCaseInsensitive { get; set; } = true;
    /// <summary>
    /// The default format that should be used for getting the query string from an URI. Default: Unescaped.
    /// </summary>
    public UriFormat DefaultQueryFormat { get; set; } = UriFormat.Unescaped;
}
