namespace TestableHttpClient;

/// <summary>
/// Options specific for URI pattern matching.
/// </summary>
public sealed class UriPatternMatchingOptions
{
    /// <summary>
    /// Indicates whether the scheme of a URI should be treated as case-insensitive. Default: true
    /// </summary>
    public bool SchemeCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether the host of a URI should be treated as case-insensitive. Default: true
    /// </summary>
    public bool HostCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether the path of a URI should be treated as case-insensitive. Default: true
    /// </summary>
    public bool PathCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether the query parameters of a URI should be treated as case-insensitive. Default: true
    /// </summary>
    public bool QueryCaseInsensitive { get; set; } = true;
    /// <summary>
    /// The default format that should be used for getting the query string from a URI. Default: Unescaped.
    /// </summary>
    public UriFormat DefaultQueryFormat { get; set; } = UriFormat.Unescaped;
}
