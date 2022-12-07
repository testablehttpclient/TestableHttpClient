namespace TestableHttpClient;

/// <summary>
/// Options specific for routing.
/// </summary>
[Obsolete("Renamed to UriPatternMatchingOptions", true)]
public class RoutingOptions
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
}
