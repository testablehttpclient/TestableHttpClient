using System.Diagnostics;

namespace TestableHttpClient.Tests.dotnet;

public class UnreachableExceptionTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultMessage()
    {
        UnreachableException exception = new();

        Assert.NotEmpty(exception.Message);
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        string message = "My exception";
        UnreachableException exception = new(message);

        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsMessageAndInnerException()
    {
        string message = "My exception";
        NotSupportedException innerException = new();
        UnreachableException exception = new(message, innerException);

        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}
