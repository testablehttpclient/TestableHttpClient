namespace TestableHttpClient;

/// <summary>
/// A set of static methods for checking values on a <see cref="HttpRequestMessage"/>.
/// </summary>
internal static class HttpRequestMessageExtensions
{
    internal static bool HasHeader(this HttpRequestMessage httpRequestMessage, string headerName)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);

        return httpRequestMessage.Headers.HasHeader(headerName) || (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.HasHeader(headerName));
    }

    internal static bool HasHeader(this HttpRequestMessage httpRequestMessage, string headerName, string headerValue)
    {
        Guard.ThrowIfNull(httpRequestMessage);
        Guard.ThrowIfNullOrEmpty(headerName);
        Guard.ThrowIfNullOrEmpty(headerValue);

        return httpRequestMessage.Headers.HasHeader(headerName, headerValue) || (httpRequestMessage.Content is not null && httpRequestMessage.Content.Headers.HasHeader(headerName, headerValue));
    }
}
