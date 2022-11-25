namespace TestableHttpClient.Utils;

/// <summary>
/// Exception thrown when the route string can not be parsed correctly.
/// </summary>
public sealed class RouteParserException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RouteParserException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public RouteParserException(string? message) : base(message)
    {
    }
}
