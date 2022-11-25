using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient;

/// <summary>
/// Exception thrown when the request assertion failed.
/// </summary>
[SuppressMessage("SonarSource", "S3925", Justification = "These exceptions don't need to be serialized.")]
public sealed class HttpRequestMessageAssertionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestMessageAssertionException"/> class with the default error message.
    /// </summary>
    public HttpRequestMessageAssertionException()
        : this("Assertion failed.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestMessageAssertionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public HttpRequestMessageAssertionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestMessageAssertionException"/>
    /// class with a specified error message and a reference to the inner exception that is the cause of
    /// this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpRequestMessageAssertionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
