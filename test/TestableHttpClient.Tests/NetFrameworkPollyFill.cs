#if NETFRAMEWORK
using System.Threading;

namespace TestableHttpClient.Tests;


internal static class NetFrameworkPollyFill
{
    public static Task<string> ReadAsStringAsync(this HttpContent content, CancellationToken cancellationToken = default)
    {
        return content.ReadAsStringAsync();
    }
}

#endif
