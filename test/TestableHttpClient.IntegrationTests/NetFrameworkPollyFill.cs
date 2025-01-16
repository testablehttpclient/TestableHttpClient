using System.Threading;

namespace TestableHttpClient.IntegrationTests;

#if NETFRAMEWORK

internal static class NetFrameworkPollyFill
{
    public static Task<string> ReadAsStringAsync(this HttpContent content, CancellationToken cancellationToken = default)
    {
        return content.ReadAsStringAsync();
    }
}

#endif
