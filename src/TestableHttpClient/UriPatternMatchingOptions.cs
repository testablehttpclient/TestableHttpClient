namespace TestableHttpClient;

/// <summary>
/// Options specific for URI pattern matching.
/// </summary>
public class UriPatternMatchingOptions
{
    /// <summary>
    /// Indicates whether or not the scheme of an URI should be treated as case insensitive. Default: true
    /// </summary>
    public bool SchemeCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether or not the host of an URI should be treated as case insensitive. Default: true
    /// </summary>
    public bool HostCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether or not the path of an URI should be treated as case insensitive. Default: true
    /// </summary>
    public bool PathCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether or not the query parameters of an URI should be treated as case insensitive. Default: true
    /// </summary>
    public bool QueryCaseInsensitive { get; set; } = true;
    /// <summary>
    /// The default format that should be used for getting the query string from an URI. Default: Unescaped.
    /// </summary>
    public UriFormat DefaultQueryFormat { get; set; } = UriFormat.Unescaped;
}
