using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class RouteParserExceptionTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultMessage()
    {
        RouteParserException exception = new();

        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        string message = "My exception";
        RouteParserException exception = new(message);

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsMessageAndInnerException()
    {
        string message = "My exception";
        NotSupportedException innerException = new();
        RouteParserException exception = new(message, innerException);

        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}
