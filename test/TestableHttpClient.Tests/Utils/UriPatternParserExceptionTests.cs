using TestableHttpClient.Utils;

namespace TestableHttpClient.Tests.Utils;

public class UriPatternParserExceptionTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultMessage()
    {
        UriPatternParserException exception = new();

        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        string message = "My exception";
        UriPatternParserException exception = new(message);

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsMessageAndInnerException()
    {
        string message = "My exception";
        NotSupportedException innerException = new();
        UriPatternParserException exception = new(message, innerException);

        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}
