#if NETFRAMEWORK

using System.Threading;

namespace TestableHttpClient.IntegrationTests;


internal static class NetFrameworkPollyFill
{
    public static Task<string> ReadAsStringAsync(this HttpContent content, CancellationToken cancellationToken = default)
    {
        return content.ReadAsStringAsync();
    }

    public static Task<string> GetStringAsync(this HttpClient client, string requestUri, CancellationToken cancellationToken = default)
    {
        return client.GetStringAsync(requestUri);
    }
}

#endif
