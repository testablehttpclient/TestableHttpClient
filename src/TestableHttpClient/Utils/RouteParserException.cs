using System.Runtime.Serialization;

namespace TestableHttpClient.Utils;

public class RouteParserException : Exception
{
    public RouteParserException()
    {
    }

    public RouteParserException(string? message) : base(message)
    {
    }

    public RouteParserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected RouteParserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
