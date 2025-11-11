namespace TestableHttpClient;

/// <summary>
/// This interface describes how responses should be implemented.
/// </summary>
public interface IResponse
{
    /// <summary>
    /// Execute the response and fill the HttpResponseMessage on the HttpResponseContext.
    /// </summary>
    /// <param name="context">The context for this request.</param>
    /// <param name="cancellationToken">The cancellationToken.</param>
    /// <returns></returns>
    public Task ExecuteAsync(HttpResponseContext context, CancellationToken cancellationToken);
}
