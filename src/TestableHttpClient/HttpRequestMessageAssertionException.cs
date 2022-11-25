namespace TestableHttpClient;

/// <summary>
/// Exception thrown when the request assertion failed.
/// </summary>
public sealed class HttpRequestMessageAssertionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestMessageAssertionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public HttpRequestMessageAssertionException(string message) : base(message)
    {
    }
}
