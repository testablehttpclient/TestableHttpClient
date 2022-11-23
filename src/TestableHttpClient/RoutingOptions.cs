namespace TestableHttpClient;

public class RoutingOptions
{
    public bool SchemeCaseInsensitive { get; set; } = true;
    public bool HostCaseInsensitive { get; set; } = true;
    public bool PathCaseInsensitive { get; set; } = true;
}
