namespace TestableHttpClient.Utils;

internal static class HttpRequestMessageCloner
{
    internal static async Task<HttpRequestMessage> ClonaAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpRequestMessage clone = new()
        {
            Method = request.Method,
            RequestUri = request.RequestUri,
            Version = request.Version,
        };

        foreach (var item in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(item.Key, item.Value);
        }

        if (request.Content is not null)
        {
            var bytes = await request.Content
                .ReadAsByteArrayAsync(cancellationToken)
                .ConfigureAwait(false);
            var contentClone = new ByteArrayContent(bytes);
            contentClone.Headers.Clear();
            // copy content headers
            foreach (var header in request.Content.Headers)
            {
                contentClone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            clone.Content = contentClone;
        }

        return clone;
    }
}
