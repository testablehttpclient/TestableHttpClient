namespace TestableHttpClient.Tests;

public sealed class RouteParserExceptionTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultMessage()
    {
        HttpRequestMessageAssertionException exception = new();

        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        string message = "My exception";
        HttpRequestMessageAssertionException exception = new(message);

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsMessageAndInnerException()
    {
        string message = "My exception";
        NotSupportedException innerException = new();
        HttpRequestMessageAssertionException exception = new(message, innerException);

        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}
