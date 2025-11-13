#if NETSTANDARD

namespace TestableHttpClient.Utils;

internal static class NetStandardPollyFill
{
    public static Task<byte[]> ReadAsByteArrayAsync(this HttpContent content, CancellationToken cancellationToken = default)
    {
        return content.ReadAsByteArrayAsync();
    }

    public static string Replace(this string input, string oldValue, string newValue, StringComparison comparisonType)
    {
        return input.Replace(oldValue, newValue);
    }
}

#endif
