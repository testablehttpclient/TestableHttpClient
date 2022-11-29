namespace TestableHttpClient;

/// <summary>
/// Options specific for Uri pattern matching.
/// </summary>
#pragma warning disable CS0618 // Type or member is obsolete, needed for backwards compatibility
public class UriPatternMatchingOptions : RoutingOptions
#pragma warning restore CS0618 // Type or member is obsolete
{
    /*
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
    */
    /// <summary>
    /// Indicates whether or not the query parameters of an URI should be treated as case insensitive. Default: true
    /// </summary>
    public bool QueryCaseInsensitive { get; set; } = true;
}
