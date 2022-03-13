using Moq;

namespace TestableHttpClient.Tests.HttpRequestMessagesCheckExtensionsTests;

public static class Its
{
    public static Func<HttpRequestMessage, bool> AnyPredicate()
    {
        return It.IsAny<Func<HttpRequestMessage, bool>>();
    }
}
