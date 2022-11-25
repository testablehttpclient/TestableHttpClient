using System.Diagnostics.CodeAnalysis;

namespace TestableHttpClient.Utils;

/// <summary>
/// Exception thrown when the route string can not be parsed correctly.
/// </summary>
[SuppressMessage("SonarSource", "S3925", Justification = "These exceptions don't need to be serialized.")]
public sealed class RouteParserException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RouteParserException"/> class with the default error message.
    /// </summary>
    public RouteParserException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteParserException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public RouteParserException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteParserException"/>
    /// class with a specified error message and a reference to the inner exception that is the cause of
    /// this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public RouteParserException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
