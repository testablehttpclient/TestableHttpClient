namespace TestableHttpClient;

/// <summary>
/// Options specific for routing.
/// </summary>
[Obsolete("Renamed to UriPatternMatchingOptions")]
public class RoutingOptions
{
    /// <summary>
    /// Indicates whether or not the scheme of a route should be treated as case insensitive. Default: true
    /// </summary>
    public bool SchemeCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether or not the host of a route should be treated as case insensitive. Default: true
    /// </summary>
    public bool HostCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indicates whether or not the path of a route should be treated as case insensitive. Default: true
    /// </summary>
    public bool PathCaseInsensitive { get; set; } = true;
}
