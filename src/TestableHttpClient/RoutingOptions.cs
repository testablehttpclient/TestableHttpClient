namespace TestableHttpClient;

/// <summary>
/// Options specific for routing.
/// </summary>
public class RoutingOptions
{
    /// <summary>
    /// Indeicates whether or not the scheme of a route should be treated as case insensitive. Default: true
    /// </summary>
    public bool SchemeCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indeicates whether or not the host of a route should be treated as case insensitive. Default: true
    /// </summary>
    public bool HostCaseInsensitive { get; set; } = true;
    /// <summary>
    /// Indeicates whether or not the path of a route should be treated as case insensitive. Default: true
    /// </summary>
    public bool PathCaseInsensitive { get; set; } = true;
}
